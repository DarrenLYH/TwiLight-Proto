using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LO_Wall : LightableObject
{
    public Collider2D objCollider;

    public override void ActivateInteraction()
    {
        ToggleWall(false);
    }

    public override void DeactivateInteraction()
    {
        ToggleWall(true);
        isTriggered = false;
    }

    public void ToggleWall(bool enabled)
    {
        if (enabled)
        {
            int newLayer = LayerMask.NameToLayer("Objects");
            gameObject.layer = newLayer;
            objCollider.isTrigger = false;
        }

        else
        {
            int newLayer = LayerMask.NameToLayer("Vis - Invis");
            gameObject.layer = newLayer;
            objCollider.isTrigger = true;
        }
    }
}
