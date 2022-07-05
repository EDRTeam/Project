using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Play : MonoBehaviour
{
    PlayableDirector Player;
    [SerializeField]
    TimelineAsset A;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayableDirector>();
        Player.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void act()
    {
        Player.Play();
    } 
}
