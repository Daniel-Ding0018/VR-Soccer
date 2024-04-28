using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegFollower : MonoBehaviour
{

    public GameObject leg;
    
    
    void FixedUpdate()
    {
        transform.rotation = leg.transform.rotation;
        transform.position = leg.transform.position;
    }
}
