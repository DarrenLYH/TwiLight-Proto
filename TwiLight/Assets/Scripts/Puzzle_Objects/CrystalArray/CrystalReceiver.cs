using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceiver : MonoBehaviour
{
    CrystalPuzzleManager PM;

    public GameObject activationLight;
    public GameObject activatedEntity; //Object triggered/affected by the Receiver
    public Sprite[] receiverStates;

    //temp
    public GameObject prefab;
    public bool isPermanent = false;
    public bool isActivated = false;

    private void Awake()
    {
        PM = GetComponentInParent<CrystalPuzzleManager>();
    }
    public void TriggerReceiver()
    {
        //Activate Effect
        activationLight.SetActive(true);

        if (!isActivated)
        {
            isActivated = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = receiverStates[1];
        }
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
