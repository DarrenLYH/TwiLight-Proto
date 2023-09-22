using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PasscodeObject : MonoBehaviour
{
    public GameObject PasscodeScreen;
    public int objPasscode;        //The specific Passcode for the Object
    protected bool isOpen = false; //Passcode Screen State

    //Object Parameters
    protected bool isTouching = false; //Player touching object
    public bool isUnlocked = false;

    // Update is called once per frame
    public void Update()
    {
        //If Object has not been unlocked and Player is interacting
        if (!isUnlocked && isTouching && Input.GetKeyDown(KeyCode.E))
        {
            ToggleScreen(isOpen);
            //insert lock sound here
        }
        
        //If Object is unlocked
        else if(isUnlocked && isTouching)
        {
            DoUnlockAction();
        }
    }

    //Toggle the Passcode Screen
    public void ToggleScreen(bool open)
    {
        PasscodeScript PS = PasscodeScreen.GetComponent<PasscodeScript>();

        if (!open)
        {
            Time.timeScale = 0;    
            PS.passcode = objPasscode; //Set Passcode to current object passcode
            PS.refObject = this;       //Set Referenced Object to current object

            //Update Screen State
            PasscodeScreen.gameObject.SetActive(true);
            isOpen = !open;
        }

        else
        {
            //Update Screen State
            isOpen = !open;
            PasscodeScreen.SetActive(false);

            PS.ResetNumber();    //clear onscreen Code
            PS.refObject = null; //clear Referenced Object
            Time.timeScale = 1;
        }
    }

    public virtual void DoUnlockAction()
    {
        //Disable Object and Close Screen
        isTouching = false;
        GameController.instance.DisplayInteractPrompt();
        ToggleScreen(isOpen);
        
        //insert unlock audio here
        Debug.Log("ya did it");
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isUnlocked)
        {
            isTouching = true;
            GameController.instance.DisplayInteractPrompt();
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (!isUnlocked)
        {
            isTouching = false;
            GameController.instance.DisplayInteractPrompt();
        }
    }
    #endregion
}