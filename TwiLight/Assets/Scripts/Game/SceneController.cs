using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ToScene(string destination)
    {
        SceneManager.LoadScene(destination);
        AudioController.instance.StopBGM();
    }

    public void ToTitle()
    {
        GameController.instance.TogglePause();  //Unpause Game
        GameController.instance.SelfDestruct(); //Reset GameController
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
