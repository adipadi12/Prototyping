using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    public Transform[] path; 
    IEnumerator currentMoveCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        string[] messages = { "Welcome", "To", "this", "amazing", "game" }; //initializing the string
        StartCoroutine(PrintLogs(messages, 0.5f)); //coroutine started which prints the messages in the above array at a delay of 0.5 seconds\
        StartCoroutine(FollowPath());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine =  MoveObject(Random.onUnitSphere * 5, 5f); //random unit sphere with a speed of 5f
            StartCoroutine(currentMoveCoroutine); //done so pressing spacebar multiple times stops the ongoing coroutine first before trying to move to the next one
            //to avoid jagged movements of the cube
        }
    }

    IEnumerator FollowPath()
    {
        foreach (Transform waypoint in path) {
            yield return StartCoroutine(MoveObject(waypoint.position, 6f)); //follows the path and waits till one waypoint is done
        }
    }

    IEnumerator MoveObject(Vector3 destination, float speed)
    {
        while (transform.position != destination) //moves to destination till position is same as destination
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime); //move position towards a destination with speed
            yield return null;
            Debug.Log(destination);
        }
                
    }
    IEnumerator PrintLogs(string[] messages, float delay)
    {
        foreach (string msg in messages)
        {
            print(msg);
            yield return new WaitForSeconds(delay); //prints messages with that delay
        }
    }
}
