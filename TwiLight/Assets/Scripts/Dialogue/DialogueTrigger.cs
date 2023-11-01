using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for Dialogue triggering in-game

public class DialogueTrigger : MonoBehaviour
{
    public string cutscene; //Dialogue Scene to Start
    public string line;     //Dialogue Line to Start
    bool isTriggered;       //Check for whether or not the dialogue has been triggered (so dialogue can only be triggered once)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            //Start Dialogue
            GameController.instance.DC.StartDialogue(cutscene, line);
            isTriggered = true;
        }
    }
}
