using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

//Class to process and convert Data to a readable form
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadRefData();
    }

    public void LoadRefData()
    {
        TextAsset dataSource = Resources.Load("dialogueJSON") as TextAsset;
        string source = dataSource.text;
        GameData gameData = JsonUtility.FromJson<GameData>(source);

        //Process Data
        ProcessGameData(gameData);
        Debug.Log("Loaded Successfully");
    }

    private void ProcessGameData(GameData gameData)
    {
        List<DialogueData> processedDialogue = new List<DialogueData>();

        //Parse RefData and convert the info into DialogueData
        foreach (DialogueRefData refData in gameData.refDialogueList)
        {
            DialogueData DialogueLine = new DialogueData(refData.dialogueLineID,
                                                         refData.nextLineID,
                                                         refData.cutsceneID,
                                                         refData.currentSpeaker,
                                                         refData.dialogue,
                                                         refData.choices,
                                                         refData.soundEffects);
            processedDialogue.Add(DialogueLine);
        }

        GameController.instance.DC.SetDialogueList(processedDialogue);
        Debug.Log("Registered Dialogue: " + GameController.instance.DC.GetDialogueList().Count);
    }
}
