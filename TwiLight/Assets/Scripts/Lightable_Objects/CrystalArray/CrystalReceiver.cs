using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalReceiver : MonoBehaviour
{
    public GameObject activationLight;
    
    //temp
    public GameObject prefab;
    bool isActivated = false;

    public void TriggerReceiver()
    {
        //Activate Effect
        activationLight.SetActive(true);

        if (!isActivated)
        {
            Instantiate(prefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
            isActivated = true;
        }
    }

    public void ShutoffReceiver()
    {
        //Deactivate Effect
        activationLight.SetActive(false);
    }
}
