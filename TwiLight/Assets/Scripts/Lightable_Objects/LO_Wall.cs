using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LO_Wall : LightableObject
{
    public Collider2D objCollider; //Wall Collider for Raycasting
    public Collider2D wallFace;    //Wall Collider for Movement(No Raycasting)

    public override void ActivateInteraction()
    {
        StartCoroutine(WallDissapear());
    }

    public override void DeactivateInteraction()
    {
        isTriggered = false;
    }

    //Coroutine ensures Player has some time to pass through
    public IEnumerator WallDissapear()
    {
        ToggleWall(false);

        yield return new WaitForSeconds(0.5f);

        while(isContacted != false)
        {
            yield return null;
        }

        ToggleWall(true);

        yield break;
    }

    //Method to toggle Wall and all associated components accordingly
    public void ToggleWall(bool enabled)
    {
        if (enabled)
        {
            int newLayer = LayerMask.NameToLayer("Objects");
            gameObject.layer = newLayer; //Raycast Cannot Pass
            
            //Player Cannot Pass
            objCollider.isTrigger = false; 
            wallFace.isTrigger = false;
        }

        else
        {
            int newLayer = LayerMask.NameToLayer("Vis - Invis");
            gameObject.layer = newLayer; //Raycast Can Pass
            
            //Player Cannot Pass
            objCollider.isTrigger = true; 
            wallFace.isTrigger = true;
        }
    }
}
