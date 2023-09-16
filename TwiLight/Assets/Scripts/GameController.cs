using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject player;
    public PlayerScript PS;

    public bool isPaused = false;

    private void Awake()
    {
        //Instantiate GameController
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        //Get Player
        player = GameObject.FindGameObjectWithTag("Player");
        PS = player.GetComponent<PlayerScript>();
    }

    public void Update()
    {
        //Pause Function Check
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(isPaused);
        }
    }

    //Increase the Player's Light Level by 1
    public void PlayerLevelUp()
    {
        PS.lightLevel += 1;
    }

    //Pause Function
    public void PauseGame(bool paused)
    {
        if (!paused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }

        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}
