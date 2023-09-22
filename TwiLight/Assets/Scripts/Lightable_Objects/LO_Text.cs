using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LO_Text : LightableObject
{
    public SpriteRenderer SR;

    private void Awake()
    {  
        //Active by Default if Lv1 Text
        if(levelRequirement <= 1)
        {
            ActivateInteraction();
          
        }
    }

    public override void ActivateInteraction()
    {
        //Enable Text to be viewed
        SR.enabled = true; 

        //Change Properties so it no longer obstructs check ray
        int newLayer = LayerMask.NameToLayer("Mask");
        gameObject.layer = newLayer;
        gameObject.tag = "Untagged";
    }

    public override void DeactivateInteraction()
    {
        //do nothing
    }
}
