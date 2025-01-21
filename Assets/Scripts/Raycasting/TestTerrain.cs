using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTerrain : MonoBehaviour
{
    public Transform objectToPlace;
    public Camera gameCamera;
    public LayerMask terrainMask;
    // Update is called once per frame
    void Update()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);  //hits a ray to a point in screen in the game camera 
        RaycastHit hitLog;

        if (Physics.Raycast(ray, out hitLog, Mathf.Infinity, terrainMask))
        {
            objectToPlace.position = hitLog.point;  //placing the object where the raycast is hitting in the game view
            objectToPlace.rotation = Quaternion.FromToRotation(Vector3.up, hitLog.normal); //uses the surface normal to
                                                                                           //move the cube according to the contours of the terrain
            Debug.DrawLine(ray.origin, objectToPlace.position, Color.red);
        }
    }
}
