using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backCollision : MonoBehaviour
{
    public ParticleSystem fireworks1;
    public ParticleSystem fireworks2;
    public AudioSource cheering;
    public AudioSource niceShot;
    
    // Start is called before the first frame update
    void Start()
    {
        fireworks1.GetComponent<ParticleSystem>();
        fireworks2.GetComponent<ParticleSystem>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball")
        {
            fireworks1.Play();
            fireworks2.Play();
            cheering.Play();
            niceShot.Play();
        }
    }
}
