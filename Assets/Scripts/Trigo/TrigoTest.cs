using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigoTest : MonoBehaviour
{
    public float angleInDegrees;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); 
        Debug.DrawRay(transform.position, direction * 3, Color.green);

        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(inputDir * Time.deltaTime * 5f, Space.World);

        float inputAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * inputAngle;
    }
}
