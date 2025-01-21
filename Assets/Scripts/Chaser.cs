using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public Transform targetTransform;
    [SerializeField]private float speed = 7f;

    // Update is called once per frame
    void Update()
    {
        Vector3 displacementFromTarget = targetTransform.position - transform.position;
        Vector3 directionToTaregt = displacementFromTarget.normalized;
        Vector3 velocity = directionToTaregt * speed;

        float distanceToTarget = displacementFromTarget.magnitude;
        if (distanceToTarget > 1.5f)
        {
            transform.Translate(velocity * Time.deltaTime);
        }
        
    }
}
