using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class DialogueController : MonoBehaviour
{
    //UI Elements
    public GameObject dialogueScreen;
    public GameObject buttonOne;
    public GameObject buttonTwo;
    public TextMeshProUGUI dialogueDisplay; //Dialogue Text
    public TextMeshProUGUI buttonOneText;   //Choice Button Content
    public TextMeshProUGUI buttonTwoText;

    //Data Elements
    private static List<DialogueData> dialogueList;                        //List of all Dialogue Data
    public List<DialogueData> currentCutscene = new List<DialogueData>();  //Specific Cutsene Contents
    public DialogueData currentDialogue;                                   //Current Dialogue Line

    public string lineContent;      //Store content of each line
    public float textSpeed;           //Speed of Typing
    string[] choiceA = new string[2]; //Arrays to store Button content
    string[] choiceB = new string[2];

    bool isChoosing; //Dialogue State

    void Update()
    {
        //if (dialogueDisplay.text == currentDialogue.dialogue) //Dialogue Completed
        {
            //stop audio
        }

        //If player clicks when:
        if (Input.GetMouseButtonDown(0) && isChoosing == false)
        {
            //dialogue is fully displayed > Start Next Line
            if (dialogueDisplay.text == currentDialogue.dialogue)
            {
                NextLine();
            }

            //display is empty > Enable Player Choice Options
            else if (dialogueDisplay.text == "")
            {
                EnableButtons();
                isChoosing = true;
            }

            //dialogue in progress > Stop Coroutines and Display Full Dialogue
            else
            {
                StopAllCoroutines();
                dialogueDisplay.text = currentDialogue.dialogue;
            }
        }
    }


    #region Dialogue Progression
    public void StartDialogue(string cutsceneID, string lineID) //Takes in the setID, RefID
    {
        //Enable Screen and Disable Player
        dialogueScreen.SetActive(true);
        InitalizeDialogue();
        GameController.instance.DisablePlayer();

        //Assigns cutscene and dialogue on run
        AssignCutscene(cutsceneID);  //NOTE** AssignCutscene must run before AssignDialogue
        AssignDialogue(lineID);

        //Starts the typing
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        //If next CRID != -1 or -2, assign next CRID and dialogue
        if (currentDialogue.nextLineID != "-1" && currentDialogue.nextLineID != "-2")
        {
            //To prevent mismatch of CRID to text
            if (isChoosing != true)
            {
                AssignDialogue(currentDialogue.nextLineID);
            }

            //Reset to default state
            isChoosing = false;
            dialogueDisplay.text = string.Empty;

            //Start next Line
            StartCoroutine(TypeLine());
        }

        //If NCRID column indicates there are choices
        else if (currentDialogue.nextLineID == "-2")
        {
            AssignChoices(currentDialogue.choices);  //Sends data to be processed
            EnableButtons();    //Makes choices buttons visible
        }

        //End dialogue and hide everything
        else
        {
            dialogueScreen.SetActive(false);
            GameController.instance.EnablePlayer();
            Debug.Log("yay");
        }
    }

    IEnumerator TypeLine()
    {
        //play sfx

        foreach (char c in lineContent.ToCharArray()) //Types out dialogue for typingCharDialogue
        {
            dialogueDisplay.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    #endregion

    #region Dialogue Processing
    //Clear Dialogue Box
    public void InitalizeDialogue()
    {
        dialogueDisplay.text = string.Empty;
        isChoosing = false;
        DisableButtons();
    }

    //Assign current Cutscene 
    public void AssignCutscene(string cutsceneID)
    {
        //Reset Current Cutscene
        currentCutscene.Clear();

        //Add line if it belongs to the specified Cutscene ID
        foreach (DialogueData line in dialogueList)
        {
            if (line.cutsceneID == cutsceneID)
            {
                currentCutscene.Add(line);
            }
        }
    }

    //Assign current Line
    public void AssignDialogue(string dialogueLineID)   //Takes in CRID column of csv
    {
        currentDialogue = currentCutscene.Find(line => line.dialogueLineID == dialogueLineID);
        lineContent = currentDialogue.dialogue; //Assign the current dialogue content to the LineContent array for printing

        if (currentDialogue.currentSpeaker == "Elio")
        {
           //display elio box
        }

        else if (currentDialogue.currentSpeaker == "Dara")
        {
            //display dara box
        }

        else
        {
          //display interaction box
        }
    }

    //Assign Choices to Button
    public void AssignChoices(string choiceInput)
    {
        //Array of choices
        string[] choices = choiceInput.Split('%'); //Splits the column into its two options with %

        //Making each choice into its own array
        //Format is Button Text [0], CutsceneID [1] split by @
        choiceA = choices[0].Split('@');
        choiceB = choices[1].Split('@');

        AssignButtonText(choiceA[0], choiceB[0]);
    }

    //Assigns button text based on input
    public void AssignButtonText(string inputA, string inputB)
    {
        dialogueDisplay.text = string.Empty; //Refreshes buttons text
        buttonOneText.SetText(inputA);       //Sets button one text
        buttonTwoText.SetText(inputB);       //Sets button two text
    }
    #endregion

    #region Button Functionality
    public void SelectChoiceA()
    {
        //Assigns next ID to the dialogue
        AssignDialogue(choiceA[1]);

        //Start next Line
        NextLine();
        DisableButtons();
    }

    public void SelectChoiceB()
    {
        //Assigns next ID to the dialogue
        AssignDialogue(choiceB[1]);

        //Start next Line
        NextLine();
        DisableButtons();
    }

    public void EnableButtons()
    {
        buttonOne.SetActive(true);
        buttonTwo.SetActive(true);
    }

    public void DisableButtons()
    {
        buttonOne.SetActive(false);
        buttonTwo.SetActive(false);
    }
    #endregion

    #region Get & Set
    public List<DialogueData> GetDialogueList()
    {
        return dialogueList;
    }

    //Update the Dialogue List
    public void SetDialogueList(List<DialogueData> input)
    {
        dialogueList = input;
    }
    #endregion
}
