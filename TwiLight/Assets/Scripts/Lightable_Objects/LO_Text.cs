using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LO_Text : LightableObject
{
    private void Awake()
    {  
        //Active by Default if Lv1 Text
        if(levelRequirement == 1)
        {
            SR.enabled = true;
        }
    }

    public SpriteRenderer SR;
    public override void ActivateInteraction()
    {
        SR.enabled = true;
    }

    public override void DeactivateInteraction()
    {
        //do nothing
    }
}
