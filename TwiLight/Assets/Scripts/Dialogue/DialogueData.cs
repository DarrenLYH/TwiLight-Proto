using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

//Class to store processed Dialogue Data
public class DialogueData
{
    public string dialogueLineID { get; }
    public string nextLineID { get; }
    public string cutsceneID { get; }
    public string currentSpeaker { get; }
    public string dialogue { get; }
    public string choices { get; }
    public string soundEffects { get; }

    //Data Constructor
    public DialogueData(string refLineID, string refNextID, string refCutsceneID, string refCurrentSpeaker, string refDialogue, string refChoices, string refSoundEffects)
    {
        this.dialogueLineID = refLineID;
        this.nextLineID = refNextID;
        this.cutsceneID = refCutsceneID;

        this.currentSpeaker = refCurrentSpeaker;
        this.dialogue = refDialogue;
        this.choices = refChoices;
        this.soundEffects = refSoundEffects;
    }
}
