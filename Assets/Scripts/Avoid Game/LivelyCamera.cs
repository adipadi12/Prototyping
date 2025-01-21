using UnityEngine;

public class LivelyCamera : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float
        shakeIntensity = 0.1f;

    Vector3 velocity;

    public void Shake()
    {
        velocity.x += Random.Range(-shakeIntensity, shakeIntensity);
        velocity.y += Random.Range(-shakeIntensity, shakeIntensity);
        velocity.z += Random.Range(-shakeIntensity, shakeIntensity);
    }

    void LateUpdate()
    {
        transform.localPosition += velocity * Time.deltaTime;
        velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 3f);
    }
}