using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public static event System.Action OnGuardHasSpotedPlayer;
    public Light spotLight;
    public float viewDist;
    public float timeToSpotPlayer = 0.5f;
    public float turnSpeed = 90;

    float viewAngle;
    float playerVisibleTimer;

    public float speed = 5f;
    public float waitTime = 0.3f;
    public LayerMask viewMask;

    public Transform pathHolder; //transform position of the path empty game object
    Transform player;
    Color originalSpotlightColor;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotLight.spotAngle;
        originalSpotlightColor = spotLight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount]; //waypoints ka array and storing the children of pathholder in this array for their vector3 positions
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position; //getting that position
            waypoints[i] = new Vector3(waypoints [i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));
    }
    private void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotLight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer/timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            if (OnGuardHasSpotedPlayer != null)
            {
                OnGuardHasSpotedPlayer();    
            }
        }
    }
    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDist)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            Debug.Log("Distance: " + Vector3.Distance(transform.position, player.position) + ", Angle: " + angleBetweenGuardAndPlayer);

            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    Debug.Log("Player is visible, turning spotlight red.");
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator FollowPath(Vector3[] waypoints) //making an array to store all the waypoints
    {
        transform.position = waypoints[0]; //making the guard's initial position as that of the first waypoint's

        int targetWaypointIndex = 1; //index of the next target in the waypoint array   
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint); //initialy faces the next waypoint

        while(true) { 
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime); //move towards target waypoint vector with speed
            if (transform.position == targetWaypoint) //when reached
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length; //increment the index by 1 and modulus it with length for remainder so it returns to 0 after reaching the intial position
                targetWaypoint = waypoints[targetWaypointIndex]; //making the next waypoint as the incremented index
                yield return new WaitForSeconds(waitTime); //waiting for 0.3 seconds then moving
                yield return StartCoroutine(TurnToFace(targetWaypoint)); //rotates after moving to the new point
            }
            yield return null;  
        }
    }
    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookAt = (lookTarget - transform.position).normalized; //normalizing the vector that is the difference of waypoint ka vector - guard ka
        float targetAngle = 90 - Mathf.Atan2(dirToLookAt.z, dirToLookAt.x) * Mathf.Rad2Deg; //creating the angle of where to look at

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f) //shortest difference between two angles done so rotation stops once < 0.5
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime); //y axis changing to make it rotate towards next waypoint only on the y axis
            transform.eulerAngles = Vector3.up * angle; // multiplying the angle above with Vector3.up so only y position gets affected
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 startPos = pathHolder.GetChild(0).position; 
        Vector3 prevousPos = startPos;

        foreach(Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(prevousPos, waypoint.position);
            prevousPos = waypoint.position;
        }
        Gizmos.DrawLine (prevousPos, startPos);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDist);
    }
}
