using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controller for Scene Switching 

public class SceneController : MonoBehaviour
{
    public void ToScene(string destination)
    {
        SceneManager.LoadScene(destination);
        AudioController.instance.StopBGM();
    }

    public void ToTitle()
    {
        AudioController.instance.PlaySFX("button", 0.05f);
        GameController.instance.TogglePause();  //Unpause Game
        GameController.instance.SelfDestruct(); //Reset GameController
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitGame()
    {
        AudioController.instance.PlaySFX("button", 0.05f);
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
