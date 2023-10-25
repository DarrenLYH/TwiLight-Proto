using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public DialogueController DC;

    //Player Components
    public GameObject player;
    public PlayerScript PS;
    public InventoryScript INV;

    //UI Elements
    public GameObject DialogueScreen;
    public GameObject PauseMenu;
    public GameObject EndScreen; //temp

    public GameObject heldItem;  
    public Sprite[] heldSprites;
    public TextMeshProUGUI levelIndicator;
    public GameObject interactPrompt;
    public GameObject pickupPrompt;

    //Game States
    public bool isPaused = false;
    public bool inScreenUI = false; //Whether Puzzle Interface is open/close

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
        //Update Components
        player = GameObject.FindGameObjectWithTag("Player");
        PS = player.GetComponent<PlayerScript>();
        INV = GetComponentInChildren<InventoryScript>();
        DC = DialogueScreen.GetComponent<DialogueController>();

        //Update UI
        DisplayHeldItem();
    }

    public void Update()
    {
        //Pause Function Check
        if (Input.GetKeyDown(KeyCode.Escape) && !inScreenUI)
        {
            TogglePause();
        }
    }

    #region Game Functions

    //Pause
    public void TogglePause()
    {
        SetPause(!isPaused);
    }

    public void SetPause(bool status)
    {
        isPaused = status;

        if (isPaused)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        else
        {
            PauseMenu.SetActive(false);
            EndScreen.SetActive(false);//temp
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    //Temporary End Screen
    public void ToggleEndScreen()
    {
            EndScreen.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
    }

    //Game Over (Currently Unused)
    public void GameOver()
    {
        Time.timeScale = 0f;
        //deathMenu.SetActive(true);
    }

    //Unload Instance when returning to Title
    public void SelfDestruct()
    {
        Destroy(gameObject);
        instance = null;
    }
    #endregion

    #region UI Functions
    public void DisplayHeldItem()
    {
        if(PS.lightLevel > 0)
        {
            int i = PS.currentLight - 1;
            heldItem.GetComponent<Image>().sprite = heldSprites[i];
        }

        levelIndicator.SetText("Current Torch: " + PS.currentLight);
    }

    public void DisplayInteractPrompt()
    {
        interactPrompt.SetActive(true);
    }

    public void HideInteractPrompt()
    {
        interactPrompt.SetActive(false);
    }

    public void DisplayPickupPrompt()
    {
        pickupPrompt.SetActive(true);
    }

    public void HidePickupPrompt()
    {
        pickupPrompt.SetActive(false);
    }
    #endregion

    #region Player Functions

    public void EnablePlayer()
    {
        PS.enabled = true;
    }

    public void DisablePlayer()
    {
        PS.enabled = false;
    }

    //Increase the Player's Light Level by 1
    public void PlayerLevelUp()
    {
        PS.lightLevel += 1;
        PS.currentLight = PS.lightLevel;
        DisplayHeldItem();
    }

    public int GetPlayerLevel()
    {
        return PS.lightLevel;
    }

    public int GetPlayerLight()
    {
        return PS.currentLight;
    }
    #endregion
}
