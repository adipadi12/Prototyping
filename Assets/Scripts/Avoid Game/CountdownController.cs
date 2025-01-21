using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public int countdownTimer;
    public TextMeshProUGUI countDownDisplay;

    public GameObject fallingBlocks;
    public GameObject Spawner;

    public AudioClip countdownSound;
    public AudioClip goSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        fallingBlocks.SetActive(false);
        Spawner.SetActive(false);
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        while (countdownTimer > 0)
        {
            countDownDisplay.text = countdownTimer.ToString();

            audioSource.PlayOneShot(countdownSound);

            yield return new WaitForSeconds(1f);

            countdownTimer--;
        }
        countDownDisplay.text = "GO!";
        audioSource.PlayOneShot(goSound);

        yield return new WaitForSeconds(1f);

        countDownDisplay.gameObject.SetActive(false);

        fallingBlocks.SetActive(true);
        Spawner.SetActive(true);
    }
}
