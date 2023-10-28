using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LO_Text : LightableObject
{
    public SpriteRenderer SR;

    private void Awake()
    {  
        //Active by Default if Lv0 Text
        if(lightRequirement == 0)
        {
            ActivateInteraction();
        }
    }

    public override void OnHitEnter()
    {
        isLit = true;
        //Check Activation Requirements
        if (lightRequirement == GameController.instance.GetPlayerLight())
        {
            isTriggered = true;
            ActivateInteraction();
        }

        else
        {
            isTriggered = false;
            DeactivateInteraction();
        }

        Debug.Log("light hit");
    }

    public override void OnHitExit()
    {
        isLit = false;
        Debug.Log("light exit");
    }

    public override void ActivateInteraction()
    {
        //Enable Text to be viewed
        SR.enabled = true; 

        //Change Properties so it no longer obstructs check ray
        int newLayer = LayerMask.NameToLayer("Mask");
        gameObject.layer = newLayer;
    }
}
