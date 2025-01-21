using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    [SerializeField]
    private LivelyCamera livelyCamera;

    [SerializeField]
    private float offsetDestroyer = 5;
   
    public ParticleSystem destructionParticles;

    public AudioClip explosionSound;
    private AudioSource audioSource;
    
    bool isDestroying = false;

    float screenHalfHeight;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        livelyCamera = GetComponentInChildren<LivelyCamera>();
        screenHalfHeight = Camera.main.orthographicSize + (transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        float speed = Random.Range(5f, 10f);
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Check destruction condition and make sure it only runs once
        if (!isDestroying && transform.position.y <= -screenHalfHeight + offsetDestroyer)
        {
            isDestroying = true; // Set flag to prevent further calls
            TriggerDestruction();
        }
    }

    void TriggerDestruction()
    {
        // Play particles if assigned
        if (destructionParticles != null)
        {
            ParticleSystem particles = Instantiate(destructionParticles, transform.position, Quaternion.identity);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }

        // Play sound and delay destruction
        audioSource.PlayOneShot(explosionSound);
        livelyCamera?.Shake();
        Destroy(gameObject, explosionSound.length); // Delayed destroy based on sound duration
    }
}
