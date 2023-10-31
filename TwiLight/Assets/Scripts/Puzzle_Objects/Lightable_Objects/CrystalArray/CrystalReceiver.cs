using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceiver : MonoBehaviour
{
    public GameObject activationLight;
    public GameObject activatedEntity; //Object triggered/affected by the Receiver
    public Sprite[] receiverStates;

    //temp
    public GameObject prefab;
    public bool isPermanent = false;
    bool isActivated = false;
    
    public void TriggerReceiver()
    {
        //Activate Effect
        activationLight.SetActive(true);

        if (!isActivated)
        {
            //temp effect implementation
            //Instantiate(prefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
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
