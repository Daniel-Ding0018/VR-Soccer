using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissCollision : MonoBehaviour
{
    public AudioSource miss;
    public ScoreManager ScoreManager;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            miss.Play();
            ScoreManager.IncrementMisses();
        }
    }
}
