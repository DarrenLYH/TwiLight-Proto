using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string cutscene;
    public string line;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameController.instance.DC.StartDialogue(cutscene, line);
    }
}
