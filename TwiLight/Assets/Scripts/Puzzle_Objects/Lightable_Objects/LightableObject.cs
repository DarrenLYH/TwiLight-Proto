using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//Parent Class for all Objects Affected by Magical Light

public abstract class LightableObject : MonoBehaviour
{
    public GameObject player;

    public int lightRequirement;     //Magic Light required to activate Object
    public bool isLit = false;       //Whether the Object is contacted by Light or not
    public bool isTriggered;         //Whether the Object has been activated / is currently activated

    public void Start()
    {
        isLit = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //If Contacted by Light
    public virtual void OnHitEnter()
    {
        isLit = true;
        //Check Activation Requirements
        if (lightRequirement == GameController.instance.GetPlayerLight())
        {
            isTriggered = true;
            ActivateInteraction();
        }
        Debug.Log("light hit");
    }

    //Light Stay
    public virtual void OnHitStay()
    {
        //Debug.Log("do nothing");
    }

    //If Not Contacted by Light
    public virtual void OnHitExit()
    {
        isLit = false;
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
