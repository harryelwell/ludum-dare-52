using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Checkpoint Tracking")]
    public int checkpointCount;
    public int lastCheckpoint;
    [Header("Race Progress")]
    public int lapCount;
    public int totalLaps;
    
    // Start is called before the first frame update
    void Start()
    {
        CountCheckpoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishLineCrossed()
    {
        if(lastCheckpoint != 0)
        {
            if(lapCount < totalLaps)
            {
                lapCount += 1;
            }

            if(lapCount >= totalLaps)
            {
                Debug.Log("Race over!");
            }
        }
    }

    public void CountCheckpoints()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");

        foreach(GameObject checkPoint in checkPoints)
        {
            checkpointCount += 1;
        }
    }
}
