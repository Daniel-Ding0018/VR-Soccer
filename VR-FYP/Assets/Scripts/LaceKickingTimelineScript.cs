using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LaceKickingTimelineScript : MonoBehaviour
{
    public PlayableDirector director;

    public void play()
    {
        director.Play();
        //Time.timeScale = (float)1;
        //Time.fixedDeltaTime = (float)0.01;
    }
}
