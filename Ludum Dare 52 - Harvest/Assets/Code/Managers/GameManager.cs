using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Object References")]
    public GameData gameData;
    public AssetLibrary assetLibrary;
    public GameObject trafficLights;
    public SpriteRenderer trafficLightsSprite;
    [Header("Gameplay Parameters")]
    public bool raceStarted;
    [Header("Checkpoint Tracking")]
    public int checkpointCount;
    public int lastCheckpoint;
    [Header("Race Progress")]
    public int lapCount;
    public int totalLaps;
    public float raceTimeElapsed;
    
    // Start is called before the first frame update
    void Start()
    {
        if(assetLibrary == null)
        {
            assetLibrary = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetLibrary>();
        }

        if(gameData == null)
        {
            gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        }

        CountCheckpoints();
        StartCoroutine(RaceCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        TrackRaceTime();
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
                //Debug.Log("Race over!");
                StartCoroutine(RaceFinish());
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
            if(secondsRemaining == 3)
            {
                trafficLightsSprite.sprite = assetLibrary.trafficLight2;
            }

            if(secondsRemaining == 2)
            {
                trafficLightsSprite.sprite = assetLibrary.trafficLight1;
            }

            if(secondsRemaining == 1)
            {
                trafficLightsSprite.sprite = assetLibrary.trafficLight0;
            }

            // Play a beep sound

            secondsRemaining -= 1;
        }

        yield return new WaitForSeconds(1);
        Debug.Log("GO! GO! GO!");
        // Start game music
        // Change sprite to green light
        trafficLightsSprite.sprite = assetLibrary.trafficLightGo;

        raceStarted = true;

        // Set movementAllowed to true for each player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerManager>().movementAllowed = true;
        }

        yield return new WaitForSeconds(1);

        Destroy(trafficLights);
    }

    void TrackRaceTime()
    {
        if(raceStarted == true)
        {
            raceTimeElapsed += Time.deltaTime;
        }
    }

    IEnumerator RaceFinish()
    {
        raceStarted = false;
        gameData.latestRaceTime = raceTimeElapsed;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(2);
    }
}
