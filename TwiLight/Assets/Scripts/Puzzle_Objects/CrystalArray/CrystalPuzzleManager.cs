using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPuzzleManager : MonoBehaviour
{
    public GameObject Activator;
    public GameObject Receiver;

    public GameObject target;//the prefab to instantiate / door to open

    private void Update()
    {
        if (Receiver.GetComponent<CrystalReceiver>().isActivated == true)
        {
            //do the thing
        }
    }
    public void UpdatePuzzle()
    {
        Activator.GetComponent<CrystalActivator>().Recalculate(); //Re-Trigger from the start
        Debug.Log("recalculating...");
    }
}
