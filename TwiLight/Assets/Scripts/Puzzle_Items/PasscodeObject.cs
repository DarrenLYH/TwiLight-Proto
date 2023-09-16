using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasscodeObject : MonoBehaviour
{
    public Canvas PasscodeScreen;
    bool isOpen = false;    //Passcode Screen State
    public int objPasscode; //The specific Passcode for the Object

    //Object Parameters
    bool inContact = false; //Player touching object
    public bool isUnlocked = false;

    // Update is called once per frame
    void Update()
    {
        //If Object has not been unlocked and Player is interacting
        if (!isUnlocked && inContact && Input.GetKeyDown(KeyCode.E))
        {
            ToggleScreen(isOpen);
        }
        
        //If Object is unlocked
        if(isUnlocked)
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
            PasscodeScreen.gameObject.SetActive(false);

            PS.ResetNumber();    //clear onscreen Code
            PS.refObject = null; //clear Referenced Object
            Time.timeScale = 1;
        }
    }

    public void DoUnlockAction()
    {
        Debug.Log("ya did it ya wanker");
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        inContact = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        inContact = false;
    }
    #endregion
}
