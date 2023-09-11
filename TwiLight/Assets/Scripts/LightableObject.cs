using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class LightableObject : MonoBehaviour
{
    public GameObject player;
    public int levelRequirement;
    public bool isContacted;
    public bool isTriggered;

    public void Start()
    {
        isContacted = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnHitEnter()
    {
        isContacted = true;
        if (levelRequirement <= player.GetComponent<PlayerScript>().lightLevel)
        {
            isTriggered = true;
            ActivateInteraction();
        }
        Debug.Log("enter dayo");
    }
    public void OnHitStay()
    {
        Debug.Log("junstin bibber lmao");
    }

    public void OnHitExit()
    {
        isContacted = false;
        DeactivateInteraction();
        Debug.Log("exits");
    }

    public virtual void ActivateInteraction()
    {

    }

    public virtual void DeactivateInteraction()
    {

    }
}
