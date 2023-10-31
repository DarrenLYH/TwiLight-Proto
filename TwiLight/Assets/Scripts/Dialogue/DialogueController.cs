using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class DialogueController : MonoBehaviour
{
    //UI Elements
    public GameObject dialogueScreen;       //Entire Screen
    public GameObject buttonOne;
    public GameObject buttonTwo;
    public GameObject elioPortrait;
    public GameObject daraPortrait;
    public GameObject nextPrompt;           //Prompt to click Next
    public TextMeshProUGUI speakerName;     //Speaker Name
    public TextMeshProUGUI dialogueDisplay; //Dialogue Text
    public TextMeshProUGUI buttonOneText;   //Choice Button Content
    public TextMeshProUGUI buttonTwoText;

    //Data Elements
    private static List<DialogueData> dialogueList;                        //List of all Dialogue Data
    public List<DialogueData> currentCutscene = new List<DialogueData>();  //Specific Cutsene Contents
    public DialogueData currentDialogue;                                   //Current Dialogue Line

    public string lineContent;        //Store content of each line
    public float textSpeed;           //Delay between each word
    string[] choiceA = new string[2]; //Arrays to store Button content
    string[] choiceB = new string[2];

    //Dialogue State
    public bool isEndDialogue = false;
    bool isChoosing;


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
        AssignCutscene(cutsceneID);
        AssignDialogue(lineID);

        //Starts the typing
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        //nextLineID "-1" > End Dialogue
        if(currentDialogue.nextLineID == "-1")
        {
            dialogueScreen.SetActive(false);
            GameController.instance.EnablePlayer();
        }

        //nextLine ID "-2" > Choices
        else if (currentDialogue.nextLineID == "-2")
        {
            AssignChoices(currentDialogue.choices); //Assign Choice Data to Buttons
            EnableButtons();                     
        }

        else //play next dialogue line
        {
            if (isChoosing != true)
            {
                AssignDialogue(currentDialogue.nextLineID);
            }

            //Clear Text & Start Next Line
            dialogueDisplay.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        //play sfx

        foreach (char c in lineContent.ToCharArray()) //Typewriter Effect
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

    //Create dataset for current Cutscene
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

    //Update Current Line Data
    public void AssignDialogue(string dialogueLineID)
    {
        currentDialogue = currentCutscene.Find(line => line.dialogueLineID == dialogueLineID);
        lineContent = currentDialogue.dialogue; //Assign the current dialogue content to the LineContent array for printing

        //Display Elio portrait
        if (currentDialogue.currentSpeaker == "Elio")
        {
            elioPortrait.SetActive(true);
            daraPortrait.SetActive(false);
            speakerName.text = "Elio";
        }

        //Display Dara portrait
        else if (currentDialogue.currentSpeaker == "Dara")
        {
            daraPortrait.SetActive(true);
            elioPortrait.SetActive(false);
            speakerName.text = "Dara";
        }

        //Display ??? 
        else if(currentDialogue.currentSpeaker == "Unknown")
        {
            daraPortrait.SetActive(true);
            elioPortrait.SetActive(false);
            speakerName.text = "???";
        }
    }

    //Assign Choices to Button
    public void AssignChoices(string choiceInput)
    {
        isChoosing = true;//Update state

        //Set Portrait to Elio
        elioPortrait.SetActive(true);
        daraPortrait.SetActive(false);
        speakerName.text = "Elio";

        string[] choices = choiceInput.Split('%'); //Splits the choices column into the two options with %

        //Split each choice into its own array
        //[0] > Button Text,[1] > nextLineID
        choiceA = choices[0].Split('@');
        choiceB = choices[1].Split('@');

        AssignButtonText(choiceA[0], choiceB[0]);
    }

    //Update Button Text
    public void AssignButtonText(string inputA, string inputB)
    {
        dialogueDisplay.text = string.Empty; //Clear button text
        buttonOneText.SetText(inputA);       //Set button one text
        buttonTwoText.SetText(inputB);       //Set button two text
    }
    #endregion

    #region Button Functionality
    public void SelectChoiceA()
    {
        //Assigns next ID to the dialogue
        AssignDialogue(choiceA[1]);

        //Start next Line
        isChoosing = false;
        StartCoroutine(TypeLine());
        DisableButtons();
    }

    public void SelectChoiceB()
    {
        //Assigns next ID to the dialogue
        AssignDialogue(choiceB[1]);

        //Start next Lines
        isChoosing = false;
        StartCoroutine(TypeLine());
        DisableButtons();
    }

    public void EnableButtons()
    {
        buttonOne.SetActive(true);
        buttonTwo.SetActive(true);
        nextPrompt.SetActive(false);
    }

    public void DisableButtons()
    {
        buttonOne.SetActive(false);
        buttonTwo.SetActive(false);
        nextPrompt.SetActive(true);
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
