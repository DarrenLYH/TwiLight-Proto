using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class LO_Wall : LightableObject
{
    public Collider2D objCollider; //Wall Collider for Raycasting
    public Collider2D wallFace;    //Wall Collider for Movement(No Raycasting)
    public GameObject areaHider;   //Darkness for obstructing view

    public override void ActivateInteraction()
    {
        StartCoroutine(WallDissapear());
    }

    public override void DeactivateInteraction()
    {
        isTriggered = false;//State = Not Triggered when looked at
    }

    //Coroutine ensures Player has some time to pass through
    public IEnumerator WallDissapear()
    {
        ToggleWall(false);

        while(isContacted != false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        
        if (isTriggered == false)
        {
            ToggleWall(true);
            yield break;
        }   
    }

    //Method to toggle Wall and all associated components accordingly
    public void ToggleWall(bool enabled)
    {
        if (enabled)
        {
            int newLayer = LayerMask.NameToLayer("Objects");
            objCollider.gameObject.layer = newLayer; //Raycast Cannot Pass
            
            //Player Cannot Pass
            objCollider.isTrigger = false; 
            wallFace.isTrigger = false;

            //Set Opacity to Full
            wallFace.gameObject.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f);
            areaHider.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 1f);
        }

        else
        {
            int newLayer = LayerMask.NameToLayer("Vis - Invis");
            objCollider.gameObject.layer = newLayer; //Raycast Can Pass
            
            //Player Cannot Pass
            objCollider.isTrigger = true; 
            wallFace.isTrigger = true;

            //Set Wall Opacity to 1/4
            wallFace.gameObject.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.25f);
            areaHider.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.25f);
        }
    }
}
