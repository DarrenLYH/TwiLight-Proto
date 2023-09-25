using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PasscodeScript : MonoBehaviour
{
    public int passcode;             //The currently stored Passcode
    public PasscodeObject refObject; //The current Object the screen is representing

    //Dictionary to store and display Code Numbers
    private Dictionary<string, int> lockNumbers; 

    //UI Elements
    public TextMeshProUGUI Status;
    public TextMeshProUGUI A;
    public TextMeshProUGUI B;
    public TextMeshProUGUI C;
    public TextMeshProUGUI D;

    private void Awake()
    {
        //Generate Dictionary
        lockNumbers = new Dictionary<string, int>
        {
            { "A", 0 },
            { "B", 0 },
            { "C", 0 },
            { "D", 0 }
        };
    }
    void Update()
    {
        CheckCode();     //Check if current code matches passcode
        DisplayNumber(); //Display the current code number
    }

    public void CheckCode()
    {
        //Compile passcode
        int checkcode = lockNumbers["A"] * 1000 + lockNumbers["B"] * 100 + lockNumbers["C"] * 10 + lockNumbers["D"];

        if(checkcode == passcode)//Match
        {
            Status.SetText("UNLOCKED!");
            refObject.isUnlocked = true;
        }

        else //No Match
        {
            Status.SetText("LOCKED");
        }
    }

    #region Lock Functions
    public void DisplayNumber()
    {
        A.SetText(lockNumbers["A"].ToString());
        B.SetText(lockNumbers["B"].ToString());
        C.SetText(lockNumbers["C"].ToString());
        D.SetText(lockNumbers["D"].ToString());
    }

    public void ResetNumber() //Reset Display to 0 0 0 0
    {
        lockNumbers["A"] = 0;
        lockNumbers["B"] = 0;
        lockNumbers["C"] = 0;
        lockNumbers["D"] = 0;
    }

    public void NumberUp(string position) //"Scroll" Number Up
    {
        //Rollover
        if (lockNumbers[position] == 9)
        {
            lockNumbers[position] = 0;
        }

        else lockNumbers[position] += 1;
    }

    public void NumberDown(string position) //"Scroll" Number Down
    {
        //Rollover
        if (lockNumbers[position] == 0)
        {
            lockNumbers[position] = 9;
        }

        else lockNumbers[position] -= 1;
    }
    #endregion
}
