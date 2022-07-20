using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/**********************************************
* 模块名: TimelineManager.cs
* 功能描述：用于Timeline播放及回调事件的执行
***********************************************/

public class TimelineManager : SceneSingleton<TimelineManager>
{
    [HideInInspector]
    public PlayableDirector playableDirector;

    public ScenarioTimeline[] scenarios;

    private PlayableDirector tempPlaybleDirector;

    /// <summary>
    /// 当前正在播放Timeline的PlayableDirector
    /// </summary>
    private List<PlayableDirector> currentPlayers = new List<PlayableDirector>();

    private void SetPD(int targetScenario)
    {
        playableDirector = scenarios[targetScenario].playableDirector;
        tempPlaybleDirector = playableDirector;
    }

    /// <summary>
    /// 播放单个timeline并执行回调action()
    /// </summary>
    /// <param name="inedx"></param>
    /// <param name="action"></param>
    /// <param name="targetScenario"> 对应脚本 </param>
    public void PlayTimeline(int targetScenario, int index, Action action = null)
    {
        SetPD(targetScenario);
        TimelineAsset asset = scenarios[targetScenario].timelines[index];
        if (asset)
        {
            Debug.Log("播放Timeline: " + asset.name);
            var playableDirectorGameObject = new GameObject(asset.name);
            var new_playableDirector = playableDirectorGameObject.AddComponent<PlayableDirector>();//播放Timeline时会临时添加一个PlaybleDirector
            new_playableDirector.extrapolationMode = DirectorWrapMode.None; //初始化
            new_playableDirector.playOnAwake = false;
            StartCoroutine(WaitTimelinePlay(asset, new_playableDirector, action));
        }
        else
        {
            action();
        }
    }

    /// <summary>
    /// 播放单个timeline并执行回调action()
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="action"></param>
    public void PlayTimeline(int targetScenario, TimelineAsset asset, Action action = null)
    {
        SetPD(targetScenario);
        if (asset)
        {
            var playableDirectorGameObject = new GameObject(asset.name);
            var new_playableDirector = playableDirectorGameObject.AddComponent<PlayableDirector>();//播放Timeline时会临时添加一个PlaybleDirector
            new_playableDirector.extrapolationMode = DirectorWrapMode.None; //初始化
            new_playableDirector.playOnAwake = false;
            StartCoroutine(WaitTimelinePlay(asset, new_playableDirector, action));
        }
        else
            action();
    }

    /// <summary>
    /// 播放多个timeline并执行回调action()
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="action"></param>
    public void PlayTimelines(int targetScenario, TimelineAsset[] timelineAssets, Action action = null)
    {
        SetPD(targetScenario);
        if (timelineAssets.Length > 0)
        {
            Debug.LogError("timelineAssets.Length > 0");
            StartCoroutine(WaitTimelinesPlay(timelineAssets, action));
            action();
        }
        else
        {
            Debug.LogError("timelineAssets.Length == 0");
            action();
        }
    }


    /// <summary>
    /// 删除所有目前播放的PlayableDirectors
    /// </summary>
    public void KillCurrentPlayingTimelines()
    {
        Debug.Log("删除Timeline： ");
        StopAllCoroutines();
        foreach (PlayableDirector player in currentPlayers)
            if (player.gameObject != this.gameObject)
            {
                player.RebuildGraph();
                Destroy(player.gameObject);
            }
        currentPlayers.Clear();
    }

    #region HELPER FUNCTION

    /// <summary>
    /// Event调用播放多个timeline的协程
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="new_playableDirector">临时创建的新playable director</param>
    /// <param name="action">回调执行的委托</param>
    /// <returns></returns>
    private IEnumerator WaitTimelinesPlay(TimelineAsset[] timelineAssets, Action action)
    {
        foreach (var timeline in timelineAssets)
        {
            var playableDirectorGameObject = new GameObject(timeline.name);
            var new_playableDirector = playableDirectorGameObject.AddComponent<PlayableDirector>();//播放Timeline时会临时添加一个PlaybleDirector
            new_playableDirector.extrapolationMode = DirectorWrapMode.None; //初始化
            new_playableDirector.playOnAwake = false;
            ResetTimelineBinding(timeline, new_playableDirector);
            new_playableDirector.Play(timeline);
            //while (new_playableDirector.state.Equals(PlayState.Playing))
            //    yield return null;

            yield return new WaitForSeconds((float)new_playableDirector.playableAsset.duration);

            if (new_playableDirector != null)
            {
                if (new_playableDirector.gameObject.activeInHierarchy)//创建的新PlayableDirector物体在场景中激活才会执行回调
                    action();
                Destroy(new_playableDirector.gameObject); //播放完毕后销毁创建的新物体
            }
        }
        action();
    }

    /// <summary>
    /// Event调用播放单个timeline的协程
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="new_playableDirector">临时创建的新playable director</param>
    /// <param name="action">回调执行的委托</param>
    /// <returns></returns>
    private IEnumerator WaitTimelinePlay(TimelineAsset asset, PlayableDirector new_playableDirector, Action action)
    {
        ResetTimelineBinding(asset, new_playableDirector);
        new_playableDirector.Play(asset);
        currentPlayers.Add(new_playableDirector);

        yield return new WaitForSeconds((float)new_playableDirector.playableAsset.duration);

        //while (new_playableDirector.state.Equals(PlayState.Playing))
        //    yield return null;

        if (new_playableDirector != null)
        {
            //创建的新PlayableDirector物体在场景中激活才会执行回调
            if (new_playableDirector.gameObject.activeInHierarchy)
            {
                if (action != null)
                {
                    action();
                }
            }

            currentPlayers.Remove(new_playableDirector);
            Destroy(new_playableDirector.gameObject); //播放完毕后销毁创建的新物体
        }
    }

    /// <summary>
    /// 获取原PlayableDirector上tracks和object的bindings并复制到新的Director上
    /// </summary>
    /// <param name="timelineAsset"></param>
    /// <param name="new_playableDirector">临时创建的新playable director</param>
    private void ResetTimelineBinding(TimelineAsset timelineAsset, PlayableDirector new_playableDirector)
    {
        tempPlaybleDirector.playableAsset = timelineAsset;
        new_playableDirector.playableAsset = timelineAsset;

        List<PlayableBinding> newBindingList = new List<PlayableBinding>();
        List<PlayableBinding> oldBindingList = new List<PlayableBinding>();

        foreach (PlayableBinding pb in tempPlaybleDirector.playableAsset.outputs)
        {
            oldBindingList.Add(pb);
        }

        foreach (PlayableBinding pb in new_playableDirector.playableAsset.outputs)
        {
            newBindingList.Add(pb);
        }

        new_playableDirector.playableAsset = timelineAsset;

        for (int i = 0; i < oldBindingList.Count; i++)
        {
            new_playableDirector.SetGenericBinding(newBindingList[i].sourceObject, tempPlaybleDirector.GetGenericBinding(oldBindingList[i].sourceObject));
        }

        tempPlaybleDirector.playableAsset = null;
    }

    #endregion HELPER FUNCTION
}