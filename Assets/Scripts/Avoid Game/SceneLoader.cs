using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void OnClickOpenMain()
    {
        SceneManager.LoadScene("GameAvoidCollision");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void OnClickOpenMedium()
    {
        SceneManager.LoadScene("GameAvoidCollisionMedium");
    }
    public void OnClickOpenHard()
    {
        SceneManager.LoadScene("GameAvoidCollisionHard");
    }
    public void OnClickOpenImp()
    {
        SceneManager.LoadScene("GameAvoidCollisionImpossible");
    }
}
