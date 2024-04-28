using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float decreaseSizeFactor = 0.9f;
    public float expandSizeFactor = 1.1f;

    private float speed = 1f;
    public GameObject[] targets;
    public Vector3[] endPosition;
    public bool setupEndPositionBool;
    public Vector3[] startPosition;
    public bool setupStartPositionBool;
    public bool[] moveToEnd;
    public bool setupBool;



    // Start is called before the first frame update
    void Start()
    {
        setupEndPositionBool = true;
        setupStartPositionBool = true;
        setupBool = true;
    }

    // Update is called once per frame
    void Update()
    {
        setupStartPosition();
        setupEndPosition();
        setupMoveBoolArray();
    }

    // randomise function for all targets
    public void randomiseTargets()
    {
        foreach(GameObject target in targets)
        {
            target.transform.localPosition = new Vector3(Random.Range(-3.16f, 3.163f), Random.Range(0.484f, 1.955f), 1.117f);
        }
        setupStartPositionBool = true;
    }

    private int getTargetIndex(string targetName)
    {
        /**
         * function to get index number for target given its name
         */
        switch (targetName)
        {
            case "Bottom Left Target":
                return 0;
            case "Top Left Target":
                return 1;
            case "Bottom Right Target":
                return 2;
            case "Top Right Target":
                return 3;
            default:
                return -1;
        }
    }

    public void setupEndPosition()
    {
        /**
         * Method used to set random end positions for each target
         */
        if (setupEndPositionBool)
        {
            endPosition = new Vector3[targets.Length];
            foreach (GameObject target in targets)
            {
                int i = getTargetIndex(target.name);
                if (i != -1)
                {
                    endPosition[i] = new Vector3(Random.Range(-3.16f, 3.163f), Random.Range(0.484f, 1.955f), 1.117f);
                }
            }
            setupEndPositionBool = false;
        }
    }

    public void setupMoveBoolArray()
    {
        /**
         * Method to set bool array to all true
         */
        if (setupBool)
        {
            moveToEnd = new bool[targets.Length];
            for (int i = 0; i < moveToEnd.Length; i++)
            {
                moveToEnd[i] = true;
            }
        }
        setupBool = false;
    }

    public void setupStartPosition()
    {
        /**
         * Method to initialise start position
         */
        if (setupStartPositionBool)
        {
            startPosition = new Vector3[targets.Length];
            foreach(GameObject target in targets)
            {
                int i = getTargetIndex(target.name);
                if(i != -1)
                {
                    startPosition[i] = target.transform.localPosition;
                }
            }
            setupStartPositionBool = false;
        }
    }

    // move targets from x = 3 to x = -3
    public void moveTarget()
    {
        /**
         * Method to handle target movement
         */
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                int i = getTargetIndex(target.name);
                Vector3 destination = moveToEnd[i] ? endPosition[i] : startPosition[i];

                // Calculate the direction to the current waypoint
                Vector3 direction = destination - target.transform.localPosition;
                direction.Normalize();

                // move the object towards the current waypoint
                target.transform.localPosition += direction * speed * Time.deltaTime;

                float distance  = Vector3.Distance(target.transform.localPosition, destination);
                if (distance < 0.1f) 
                {
                    moveToEnd[i] = !moveToEnd[i];
                }
            }
        }
    }

    public void increaseTargetMovementSpeed()
    {
        speed += 1;
    }

    public void decreaseTargetMovementSpeed()
    {
       
        if (speed > 0)
        {
            speed -= 1;
        }
    }



    public void ShrinkTargets()
    {
        foreach (GameObject target in targets)
        {
            
            // Scale down the target by resizeFactor
            target.transform.localScale *= decreaseSizeFactor;
            Debug.Log("ShrinkTargets()");
            
        }
    }
    public void ExpandTargets()
    {
        foreach (GameObject target in targets)
        {
            
            // Scale down the target by resizeFactor
            target.transform.localScale *= expandSizeFactor;
            Debug.Log("ShrinkTargets()");
            
        }
    }
}
