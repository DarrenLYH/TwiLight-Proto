using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceiver : MonoBehaviour
{
    CrystalPuzzleManager PM;

    public GameObject activationLight;
    public Sprite[] receiverStates;

    public GameObject activatedEntity = null; //Object triggered/affected by the Receiver
    public GameObject entityCollider = null;
    public Sprite doorOpen;

    public bool isPermanent = false;
    public bool isActivated = false;
    public bool eventTriggered = false;

    private void Awake()
    {
        PM = GetComponentInParent<CrystalPuzzleManager>();
    }
    public void TriggerReceiver()
    {
        //Activate Effect
        activationLight.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().sprite = receiverStates[1];
        isActivated = true;
        AudioController.instance.PlaySFX("levelup", 1f);

        if (activatedEntity != null && !eventTriggered)
        {
            eventTriggered = true;

            AudioController.instance.PlaySFX("doorOpen", 1f);
            activatedEntity.GetComponent<SpriteRenderer>().sprite = doorOpen;
            activatedEntity.GetComponent<BoxCollider2D>().enabled = false;
            entityCollider.GetComponent<BoxCollider2D>().enabled = false;
        }

        PM.CheckPuzzleState();
    }

    public void ShutoffReceiver()
    {
        if (!isPermanent)
        {
            //Deactivate Effect
            isActivated = false;
            activationLight.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().sprite = receiverStates[0];
        }
    }
}
