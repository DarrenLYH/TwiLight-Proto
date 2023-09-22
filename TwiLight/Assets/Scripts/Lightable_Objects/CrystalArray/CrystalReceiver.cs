using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceiver : MonoBehaviour
{
    public GameObject activationLight;

    public void TriggerReceiver()
    {
        //Activate Effect
        activationLight.SetActive(true);
    }

    public void ShutoffReceiver()
    {
        //Deactivate Effect
        activationLight.SetActive(false);
    }
}
