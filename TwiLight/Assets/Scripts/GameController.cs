using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject player;
    public PlayerScript PS;

    //UI Elements
    public GameObject heldItem;
    public Sprite[] heldSprites;
    public TextMeshProUGUI levelIndicator;
    public GameObject interactPrompt;
    bool ipActive = false;
    public GameObject pickupPrompt;
    bool ppActive = false;

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

        //Update UI
        DisplayHeldItem();
        DisplayLightLevel();
    }

    public void Update()
    {
        //Pause Function Check
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(isPaused);
        }
    }

    #region Game Functions

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
    #endregion

    #region UI Functions
    public void DisplayHeldItem()
    {
        int i = PS.lightLevel - 1;
        heldItem.GetComponent<Image>().sprite = heldSprites[i];
    }

    public void DisplayLightLevel()
    {
        levelIndicator.SetText("Light Level: " + PS.lightLevel);   
    }

    public void DisplayInteractPrompt()
    {
        if (!ipActive)
        {
            interactPrompt.SetActive(true);
        }

        else
        {
            interactPrompt.SetActive(false);
        }

        ipActive = !ipActive;
    }

    public void DisplayPickupPrompt()
    {
        if (!ppActive)
        {
            pickupPrompt.SetActive(true);
        }

        else
        {
            pickupPrompt.SetActive(false);
        }

        ppActive = !ppActive;
    }

    #endregion

    #region Player Functions
    //Increase the Player's Light Level by 1
    public void PlayerLevelUp()
    {
        PS.lightLevel += 1;
        DisplayLightLevel();
    }
    #endregion
}
