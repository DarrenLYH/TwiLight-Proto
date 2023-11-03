using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

//Script for Objects using a passcode lock

public class PasscodeObject : MonoBehaviour
{
    public GameObject PasscodeScreen;
    public int objPasscode;        //The specific Passcode for the Object
    protected bool isOpen = false; //Passcode Screen State

    //Object Parameters
    protected bool isTouching = false;  //Check if player touching object
    public bool isSolved = false;       //Puzzle State
    public bool eventTriggered = false; //If object has been unlocked

    public void Update()
    {
        //If Object has not been unlocked and Player is interacting
        if (!eventTriggered)
        {
            if (isTouching && Input.GetKeyDown(KeyCode.E))
            {
                //Cheat Code to Activate Item Immediately
                if (objPasscode == 6969)
                {
                    DoUnlockAction();
                }

                ToggleScreen(isOpen);
                //insert lock sound here
            }

            //If Object is unlocked
            else if (isSolved && isTouching)
            {
                DoUnlockAction();
            }
        }
    }

        //Toggle the Passcode Screen
        public void ToggleScreen(bool open)
        {
            PasscodeScript PS = PasscodeScreen.GetComponent<PasscodeScript>();

            if (!open)
            {
                PS.passcode = objPasscode; //Set Passcode to current object passcode
                PS.refObject = this;       //Set Referenced Object to current object
                PS.DisplayScreen();        //Update Screen State
                isOpen = !open;
            }

            else
            {
                //Update Screen State
                isOpen = !open;
                PS.HideScreen();
            }
        }

    public virtual void DoUnlockAction()
    {
        //Disable Object and Close Screen
        eventTriggered = true;
        isTouching = false;

        ToggleScreen(isOpen);

        //insert unlock audio here
        Debug.Log("ya did it");
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!eventTriggered)
        {
            isTouching = true;
            GameController.instance.DisplayInteractPrompt();
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        GameController.instance.HideInteractPrompt();
    }
    #endregion
}
