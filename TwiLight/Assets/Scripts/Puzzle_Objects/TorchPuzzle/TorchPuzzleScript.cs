using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorchPuzzleScript : MonoBehaviour
{
    public PuzzleTorch[] torchSet;
    public GameObject door;
    public GameObject doorBlocker;
    public Sprite doorOpen;

    public bool isSolved = false;

    public void CheckPuzzleStatus()
    {
        int counter = torchSet.Count() / 2;
        int i2 = counter;

        for (int i = 0; i < counter; i++)
        {
            
            if (torchSet[i].isLit == torchSet[i2].isLit)
            {
                i2++;
            }

            else
            {
                Debug.Log("this aint it chief");
                return;
            }
        }

        Debug.Log("yeah that checks out");
        ActivateEffect();
    }

    public void ActivateEffect()
    {
        foreach (PuzzleTorch x in torchSet)
        {
            x.isInteractible = false;
        }

        isSolved = true;
        AudioController.instance.PlaySFX("doorOpen", 0.25f);
        door.GetComponent<SpriteRenderer>().sprite = doorOpen;
        doorBlocker.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<BoxCollider2D>().enabled = false;
    }
}
