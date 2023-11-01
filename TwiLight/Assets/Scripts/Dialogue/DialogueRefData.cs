using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to store Dialogue information(JSON)

[System.Serializable]
public class DialogueRefData
{
    public string dialogueLineID; //UID for Dialogue Line
    public string nextLineID;     //UID of Next Line
    public string cutsceneID;     //ID of Cutscene that the line belongs to

    public string currentSpeaker; //Which Dialogue Box to show
    public string dialogue;       //Dialogue Content
    public string choices;        //Dialogue Reply Choices
    public string soundEffects;   //SFX to play
}
