using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("Gameplay Parameters")]
    public bool raceStarted;
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
        StartCoroutine(RaceCountdown());
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

    public IEnumerator RaceCountdown()
    {
        int secondsRemaining = 3;

        yield return new WaitForSeconds(1);

        while(secondsRemaining > 0)
        {
            yield return new WaitForSeconds(1);

            Debug.Log($"Race starts in... {secondsRemaining}");
            // Change lights sprite to next level down
            // Play a beep sound

            secondsRemaining -= 1;
        }

        yield return new WaitForSeconds(1);
        Debug.Log("GO! GO! GO!");
        // Start game music
        // Change sprite to green light

        raceStarted = true;

        // Set movementAllowed to true for each player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerManager>().movementAllowed = true;
        }
    }
}
