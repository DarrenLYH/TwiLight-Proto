using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Overall Game Systems Controller 
//UI elements, Game State Functions & Controls

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public DialogueController DC;

    //Player Components
    public GameObject player;
    public PlayerScript PS;
    public InventoryScript INV;

    bool noclipToggle = false;

    //UI Elements
    public GameObject DialogueScreen;
    public GameObject PauseMenu;
    public GameObject EndScreen; //temp

    public Sprite[] heldSprites;
    public GameObject heldItem;
    public GameObject hintSwitch;
    public TextMeshProUGUI hintActivate;
    public TextMeshProUGUI levelIndicator;
    public GameObject interactPrompt;
    public GameObject pickupPrompt;

    public GameObject dialogueTriggers;
    bool dialogueToggle = false;

    public TextMeshProUGUI debugText;

    //Game States
    public bool isPaused = false;
    public bool inScreenUI = false; //Whether Puzzle Interface is opened/closed

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

        //Update Components
        player = GameObject.FindGameObjectWithTag("Player");
        PS = player.GetComponent<PlayerScript>();
        INV = GetComponentInChildren<InventoryScript>();
        DC = DialogueScreen.GetComponent<DialogueController>();

        //Update UI
        PS.animator.SetInteger("CurrentLight", PS.currentLight);
        DisplayHeldItem();

        debugText.gameObject.SetActive(false);
    }

    public void Update()
    {
        //Pause Function Check
        if (Input.GetKeyDown(KeyCode.Escape) && !inScreenUI)
        {
            TogglePause();
        }

        #region Cheats
        //Max Level Player
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            PS.lightLevel = 3;
            PS.currentLight = PS.lightLevel;
            DisplayHeldItem();

            PS.levelupAnimator.SetTrigger("LevelUp");
            AudioController.instance.PlaySFX("levelup", 1f);

            debugText.SetText("Player Level Maxed");
            debugText.gameObject.SetActive(true);
            Invoke("FlashDebugText", 3f);
        }

        //Disable Dialogue
        if (Input.GetKeyDown(KeyCode.Period))
        {
            dialogueTriggers.SetActive(dialogueToggle);
            debugText.SetText("Dialogue Triggers set to " + dialogueToggle);
            dialogueToggle = !dialogueToggle;

            
            debugText.gameObject.SetActive(true);
            Invoke("FlashDebugText", 3f);
        }

        //Toggle NoClip
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            PS.GetComponent<CapsuleCollider2D>().enabled = noclipToggle;
            noclipToggle = !noclipToggle;

            debugText.SetText("NoClip set to " + noclipToggle);
            debugText.gameObject.SetActive(true);
            Invoke("FlashDebugText", 3f);
        }
        #endregion
    }

    #region Game Functions

    //Pause
    public void TogglePause()
    {
        SetPause(!isPaused);
        AudioController.instance.PlaySFX("button", 0.05f);
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

    //Unload Instance when returning to Title
    public void SelfDestruct()
    {
        Destroy(gameObject);
        instance = null;
    }
    #endregion

    #region UI Functions
    //Update UI Display
    public void DisplayHeldItem()
    {
        //Update Current Held Item
        if(PS.lightLevel > 0)
        {
            int i = PS.currentLight - 1;
            heldItem.GetComponent<Image>().sprite = heldSprites[i];
        }

        //Display Control Hints
        if (PS.lampPlaced)
        {
            hintSwitch.SetActive(false);
        }

        if (PS.currentLight == 2)
        {
            hintActivate.SetText("LMB");
        }

        if (PS.currentLight == 3)
        {
            hintActivate.SetText("E");
            hintSwitch.SetActive(true);
        }

        //Unused Debugging UI
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

    public void FlashDebugText()
    {
        debugText.gameObject.SetActive(false);
        debugText.SetText("");
    }
    #endregion

    #region Player Functions

    //Enable Player Script/Input
    public void EnablePlayer()
    {
        PS.enabled = true;
        INV.enabled = true;
    }

    //Disable Player Script/Input
    public void DisablePlayer()
    {
        PS.animator.SetFloat("Speed", 0);
        PS.FC.isWalking = false;
        PS.enabled = false;
        INV.enabled = false;
    }

    //Increase the Player's Light Level by 1
    public void PlayerLevelUp()
    {
        if(PS.lightLevel < 3)
        {
            PS.lightLevel += 1;
            PS.currentLight = PS.lightLevel;
            DisplayHeldItem();

            PS.levelupAnimator.SetTrigger("LevelUp");
            AudioController.instance.PlaySFX("levelup", 1f);
        }
    }

    //Get player's overall Level
    public int GetPlayerLevel()
    {
        return PS.lightLevel;
    }

    //Get player's current held Light
    public int GetPlayerLight()
    {
        return PS.currentLight;
    }
    #endregion
}
