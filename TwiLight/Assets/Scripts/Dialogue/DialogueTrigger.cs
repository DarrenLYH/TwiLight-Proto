using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string cutscene;
    public string line;
    bool isTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTriggered)
        {
            GameController.instance.DC.StartDialogue(cutscene, line);
            isTriggered = true;
        }
    }
}
