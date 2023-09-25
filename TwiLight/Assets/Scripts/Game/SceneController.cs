using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ToGame()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void ToTitle()
    {
        GameController.instance.TogglePause();
        GameController.instance.SelfDestruct();
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
