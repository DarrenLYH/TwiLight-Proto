using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPuzzleManager : MonoBehaviour
{
    public GameObject Activator;
    public GameObject[] Receivers;

    public GameObject target;//the prefab to instantiate / door to open
    public GameObject targetCollider;
    public Sprite doorOpen;
    bool eventTriggered = false;

    public void CheckPuzzleState()
    {
        if (Receivers.Length > 1)
        {
            foreach (GameObject CR in Receivers)
            {
                if (CR.GetComponent<CrystalReceiver>().isActivated == false)
                {
                    Debug.Log("Puzzle Unsolved");
                    return;
                }

                eventTriggered = true;
            }

            if (eventTriggered)
            {
                AudioController.instance.PlaySFX("levelup", 1f);
                target.GetComponent<SpriteRenderer>().sprite = doorOpen;
                target.GetComponent<BoxCollider2D>().enabled = false;
                targetCollider.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void UpdatePuzzle()
    {
        Activator.GetComponent<CrystalActivator>().Recalculate(); //Re-Trigger from the start
        Debug.Log("recalculating...");
    }
}
