using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthPLayer : MonoBehaviour
{
    public System.Action OnReachedEndOfLevel;

    public float moveSpeed = 7f;
    // Start is called before the first frame update
    public float smoothMagTime = 0.1f;
    public float turnSpeed = 8f;

    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody rb;

    bool disabled;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Guard.OnGuardHasSpotedPlayer += Disable;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 inputDir = Vector3.zero;
        if (!disabled)
        {
            inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized; //always normalize directions
        }
        float inputMag = inputDir.magnitude;
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMag, ref smoothMoveVelocity, smoothMagTime);

        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z)  * Mathf.Rad2Deg;
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * turnSpeed * inputMag);
        
        velocity = transform.forward * moveSpeed * smoothInputMagnitude;
    }
    void Disable()
    {
        disabled = true;
    }
    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up *angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Disable();
            if (OnReachedEndOfLevel != null)
            {
                OnReachedEndOfLevel();
            }
        }
    }
    void OnDestroy()
    {
        Guard.OnGuardHasSpotedPlayer -= Disable;    
    }
}
