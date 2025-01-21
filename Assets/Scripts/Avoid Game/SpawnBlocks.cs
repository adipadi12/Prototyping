using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField] private GameObject TextBlock;
    [SerializeField] private TextMeshProUGUI secondsSurvivedText;
    [SerializeField] private ParticleSystem fallingBlockParticles;

    public GameObject Block;
    bool gameOver;
    public Vector2 delaySecondsMinMax;
    float nextSpawnTime;

    public AudioClip playerDestroySound;
    private AudioSource audioSource;

    public Vector2 spawnSizeMinMax;

    Vector3 screenHalfSize;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        screenHalfSize = new Vector3(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize, 0);
        FindObjectOfType<PlayerAvoid>().OnPlayerDeath += GameOver; //subscribing an event with a method to be used so that it isn't called in a class that has no relation to it
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            float secondsBetweenSpawn = Mathf.Lerp(delaySecondsMinMax.y, delaySecondsMinMax.x, Difficulty.GetDiffcultyPercentage());
            print(secondsBetweenSpawn);
            nextSpawnTime = Time.time + secondsBetweenSpawn;

            float spawnSize = Random.Range(spawnSizeMinMax.x, spawnSizeMinMax.y);
            Vector3 spawnPos = new Vector3(Random.Range(-screenHalfSize.x + Block.transform.position.x /2, screenHalfSize.x - Block.transform.localScale.x / 2), screenHalfSize.y * 2, 0);
            GameObject newBlock = (GameObject)Instantiate(Block, spawnPos, Quaternion.Euler(0, 0, Random.Range(-15f, 15f))); //getting a random rotation in the z axis for the falling blocks
            newBlock.transform.localScale = Vector3.one * spawnSize;

            FallingBlocks fallingBlocks = newBlock.GetComponent<FallingBlocks>();
            if (fallingBlocks != null)
            {
                fallingBlocks.destructionParticles = fallingBlockParticles;
            }
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Jump"))
            {
                SceneManager.LoadScene("Menu AG");
            }
        }
    }

    public void GameOver()
    {
        secondsSurvivedText.text = Mathf.Round(Time.timeSinceLevelLoad - 4).ToString(); //used so that the timer is resetted everytime the level is loaded for accurate high scores
        TextBlock.SetActive(true);
        gameOver = true;
        audioSource.PlayOneShot(playerDestroySound);
        Debug.Log("Playing death sound");
    }
}
