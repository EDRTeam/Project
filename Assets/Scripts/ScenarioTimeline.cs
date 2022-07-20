using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ScenarioTimeline : MonoBehaviour
{
    //public int targetSprite;
    public TimelineAsset[] timelines;
    [HideInInspector]
    public PlayableDirector playableDirector;

    private void Awake()
    {
        if (!playableDirector)
        {
            playableDirector = gameObject.GetComponent<PlayableDirector>();
        }
    }
}
