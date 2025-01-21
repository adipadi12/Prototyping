using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLose;
    public GameObject gameWin;
    bool gameIsOver;
    // Start is called before the first frame update
    void Start()
    {
        Guard.OnGuardHasSpotedPlayer += ShowLoseUI;
        FindObjectOfType<StealthPLayer>().OnReachedEndOfLevel += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
            
        }
    }

    void ShowGameWinUI()
    {
        OnGameOver(gameWin);
    }
    void ShowLoseUI()
    {
        OnGameOver(gameLose);
    }
    void OnGameOver(GameObject gb)
    {
        gb.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpotedPlayer -= ShowLoseUI;
        FindObjectOfType<StealthPLayer>().OnReachedEndOfLevel -= ShowGameWinUI;
    }
}
