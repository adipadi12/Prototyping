using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public  LayerMask mask;
   //Raycast code
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward); //initializing a ray that moves forward
        RaycastHit hitLog; //checking if it hits or not

        if(Physics.Raycast(ray, out hitLog, 100, mask, QueryTriggerInteraction.Ignore)) //Raycasting here using parameters as the initialized ray, out keyword for referencing the
                                                  //hitLog without initializing it and 100 for max units it can forward for
                                                  //mask and query trigger used according to need. Ignore used when you want to ignore a trigger. Collide when register one
        {
            print(hitLog.collider.gameObject.name); //prints name of object with which the raycast is colliding
            Destroy(hitLog.transform.gameObject); //destroys that game object
            Debug.DrawLine(ray.origin, hitLog.point, Color.red);
        } else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);
        }
    }
}
