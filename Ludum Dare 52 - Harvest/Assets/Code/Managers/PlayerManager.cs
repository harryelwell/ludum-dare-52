using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Object References")]
    public PlayerActions playerActions;
    public Rigidbody2D playerRigidbody;
    [Header("Player Settings")]
    public float playerSpeed;
    public float turnSpeed;
    public float normalDrag;
    public float turnDrag;
    public float cornValue; // min 0, max 3 (+0.3 each time you hit corn)
    [Header("Gameplay Checks")]
    float previousTurnValue;
    IEnumerator activeDragCoroutine;
    
    void Awake()
    {
        playerActions = new PlayerActions();

        playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
        {
            Debug.Log("Rigidbody2D is null!");
        }
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        PlayerAcceleration();
        PlayerRotation();
    }

    void PlayerAcceleration()
    {
        float calculatedPlayerSpeed = playerSpeed * (1 + cornValue);
        //Debug.Log($"cornValue is {cornValue}, playerSpeed is {playerSpeed}, calculatedPlayerSpeed {calculatedPlayerSpeed}");

        float accelerationValue = playerActions.PlayerMap.Accelerate.ReadValue<float>();

        if(accelerationValue > 0f)
        {
            playerRigidbody.AddForce(transform.up * calculatedPlayerSpeed,ForceMode2D.Force);
        }
        else if(accelerationValue < 0f)
        {
            playerRigidbody.AddForce(-transform.up * (calculatedPlayerSpeed * 0.3f),ForceMode2D.Force);
        }
        else
        {
            //Debug.Log($"No clear acceleration input, do nothing!");
        }
    }

    void PlayerRotation()
    {
        float turnValue = playerActions.PlayerMap.Turn.ReadValue<float>();
        //Debug.Log($"Turn value is: {turnValue}, previous turn value is {previousTurnValue}");
        
        if(previousTurnValue != turnValue)
        {
            Debug.Log($"Turn value is: {turnValue}, previous turn value is {previousTurnValue}");
            
            if(turnValue != 0f)
            {
                //playerRigidbody.drag = turnDrag;
                Debug.Log("Apply additional drag!");
                if(activeDragCoroutine != null)
                {
                    StopCoroutine(activeDragCoroutine);
                }
                activeDragCoroutine = ApplyTurnDrag(playerRigidbody.drag,turnDrag,CalculatedTurnDragDuration(normalDrag,turnDrag));
                StartCoroutine(activeDragCoroutine);
            }

            if(turnValue == 0f)
            {
                //playerRigidbody.drag = normalDrag;
                Debug.Log("Stop applying drag and return to normal.");
                if(activeDragCoroutine != null)
                {
                    StopCoroutine(activeDragCoroutine);
                }
                activeDragCoroutine = ApplyTurnDrag(playerRigidbody.drag,normalDrag,CalculatedTurnDragDuration(turnDrag,normalDrag));
                StartCoroutine(activeDragCoroutine);
            }
        }

        // reverse the rotation if player is holding reverse
        float calculatedTurnValue = turnValue;
        if(playerActions.PlayerMap.Accelerate.ReadValue<float>() == -1f)
        {
            calculatedTurnValue = -turnValue;
        }

        transform.Rotate(0,0,calculatedTurnValue * turnSpeed * Time.deltaTime);

        previousTurnValue = turnValue;
    }

    float CalculatedTurnDragDuration(float intendedStartingDrag, float targetDrag)
    {
        float calculatedDuration = 1f;

        float expectedDifference = targetDrag - intendedStartingDrag;
        float actualDifference = playerRigidbody.drag - intendedStartingDrag;
        float progressComplete = actualDifference / expectedDifference;
        float amountToSubtract = calculatedDuration * progressComplete;

        calculatedDuration -= amountToSubtract; 

        return calculatedDuration;
    }

    IEnumerator ApplyTurnDrag(float startingDrag, float endingDrag, float lerpDuration)
    {
        Debug.Log($"Starting ApplyTurnDrag coroutine. Values are startingDrag: {startingDrag}, endingDrag: {endingDrag}, lerpDuration: {lerpDuration}.");
        float timeElapsed = 0f;
        while(timeElapsed < lerpDuration)
        {
            playerRigidbody.drag = Mathf.Lerp(startingDrag,endingDrag,timeElapsed);
            timeElapsed = timeElapsed + Time.deltaTime / lerpDuration;
            yield return new WaitForEndOfFrame();
        }
        
        playerRigidbody.drag = endingDrag;
    }
}
