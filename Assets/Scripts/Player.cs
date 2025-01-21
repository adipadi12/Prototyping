using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //mini-game to press spacebar in time
    float roundDelayTime = 0.5f;
    bool roundStarted;

    float roundStartTime;
    float waitTime;
    // Start is called before the first frame update
    void Start()
    {
        print("Press Spacebar after allotted time is up.");
        Invoke("SetRandomTime", roundDelayTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && roundStarted)
        {
            InputReceiver();
        }
    }

    void InputReceiver()
    {
        roundStarted = false;
        float playerWaitTime = Time.time - roundStartTime;
        float error = Mathf.Abs(waitTime - playerWaitTime);

        print("You waited for " + playerWaitTime + ". You were " + error + " seconds far off. " + GenerateMessage(error));
        
        Invoke("SetRandomTime", roundDelayTime);
    }

    string GenerateMessage(float error)
    {
        string message = "";
        if (error < 0.5f)
        {
            message = "Outstanding";
        }
        else if (error < 0.75f)
        {
            message = "Exceeds expectations";
        }
        else if(error < 1.25f)
        {
            message = "acceptable";
        }
        else
        {
            message = "Dreadful";
        }

        return message;
    }

    void SetRandomTime()
    {
        waitTime = Random.Range(5f, 15f);
        roundStartTime = Time.time;
        roundStarted = true;

        Debug.Log(waitTime + " Seconds");
    }
}
