using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{

    [SerializeField] private Transform sphereTransform;
    // Start is called before the first frame update
    void Start()
    {
        sphereTransform.parent = transform;
        sphereTransform.localScale = Vector3.one * 2 ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 180);
        transform.Translate(Vector3.forward * Time.deltaTime * 7, Space.World);
        //transform.eulerAngles += Vector3.up *  180 * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sphereTransform.localPosition = Vector3.zero;
        }
    }
}