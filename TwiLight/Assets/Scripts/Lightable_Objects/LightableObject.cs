using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class LightableObject : MonoBehaviour
{
    public GameObject player;
    public int levelRequirement;     //Magic Light Level required to activate Object
    public bool isContacted = false; //Whether the Object is contacted by Light or not
    public bool isTriggered;         //Whether the Object has been activated / is currently activated

    public void Start()
    {
        isContacted = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //If Contacted by Light
    public virtual void OnHitEnter()
    {
        isContacted = true;
        //Check Activation Requirements
        if (levelRequirement <= player.GetComponent<PlayerScript>().lightLevel)
        {
            isTriggered = true;
            ActivateInteraction();
        }
        Debug.Log("light hit");
    }

    //Light Stay
    public virtual void OnHitStay()
    {
        Debug.Log("do nothing");
    }

    //If Not Contacted by Light
    public virtual void OnHitExit()
    {
        isContacted = false;
        DeactivateInteraction();
        Debug.Log("light exit");
    }

    public virtual void ActivateInteraction()
    {
        //placeholder
    }

    public virtual void DeactivateInteraction()
    {
        //placeholder
    }
}
