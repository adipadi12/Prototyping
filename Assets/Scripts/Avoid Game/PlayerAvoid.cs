using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAvoid : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private ParticleSystem playerDestroyParticleSystem = default;
    public event System.Action OnPlayerDeath;

    float screenHalfWidth;
    float screenHalfHeight;
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize + (transform.localScale.x / 2); //adding half the player's width to make wraping look more seamless
        screenHalfHeight = Camera.main.orthographicSize + (transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 direction = input.normalized;
        velocity = direction * speed * Time.deltaTime;

        transform.Translate(velocity);

        if (transform.position.x > screenHalfWidth)
        {
            transform.position = new Vector3(-screenHalfWidth, transform.position.y);
        }
        if (transform.position.x < -screenHalfWidth)
        {
            transform.position = new Vector3(screenHalfWidth, transform.position.y);
        }

        if (transform.position.y >= screenHalfHeight)
        {
            transform.position = new Vector3(transform.position.x, -screenHalfHeight);
        }
        else if (transform.position.y < -screenHalfHeight)
        {
            transform.position = new Vector3(transform.position.x, screenHalfHeight);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Falling Block") 
        {
            if (OnPlayerDeath != null)
            {
                OnPlayerDeath(); //calling the event here that in turn calls the GameOver method
            }
            playerDestroyParticleSystem.transform.position = transform.position;
            playerDestroyParticleSystem.Play();
            
            Destroy(gameObject);
        }
    }
}
