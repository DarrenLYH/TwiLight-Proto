using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LO_CrystalActivator : LightableObject
{
    [SerializeField] private LayerMask contactCheck;
    public LineRenderer LR;
    public int beamDirection;
    Vector2 direction;

    public bool isToggled;

    public delegate void onActivation();

    public void LateUpdate()
    {
        if (isToggled)
        {
            EmitBeam();
        }

        else
        {
            isTriggered = false;
            StartCoroutine(DelayedShutoff());
        }
    }

    public void ToggleActivator()
    {
        isToggled = !isToggled;
    }

    public override void OnHitEnter()
    {
        isContacted = true;
        //Check Activation Requirements
        if (lightRequirement <= player.GetComponent<PlayerScript>().lightLevel)
        {
            ToggleActivator();
        }

        Debug.Log("Crystal Activated");
    }

    //If Not Contacted by Light
    public override void OnHitExit()
    {
        if (isTriggered)
        {
            isTriggered = false;
            StartCoroutine(DelayedShutoff());
        }

        isContacted = false;
        Debug.Log("Crystal Deactivated");
    }

    #region Beam Emitter
    public void EmitBeam()
    {
        isTriggered = true;
        LR.positionCount = 2;

        switch (beamDirection)
        {
            //Beam Up
            case 0:
                direction = Vector2.up;
                break;
                

            //Beam Down
            case 1:
                direction = Vector2.down;
                break;

            //Beam Left
            case 2:
                direction = Vector2.left;
                break;

            //Beam Right
            case 3:
                direction = Vector2.right;
                break;

            default:
                break;
        }
                
        //Raycast in the Beam Direction to Check Collision
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 20f, contactCheck);

        //If Colliding with Pylon > Trigger
        if (hit.collider && hit.collider.CompareTag("Pylon"))
        {
            LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
            hit.collider.gameObject.SendMessage("TriggerPylon");
            Debug.Log("pew pew");
        }

        else if (hit.collider && hit.collider.CompareTag("Receiver"))
        {
            LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
            hit.collider.gameObject.SendMessage("TriggerReceiver");
        }

        //If Colliding with Wall > Beam to Wall
        else if (hit.collider && hit.collider.CompareTag("Wall"))
        {
            LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
        }

        else
        {
            Debug.Log("nothing here");
        }
    }

    public void Shutoff()
    {
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 20f, contactCheck);

        //If Colliding with Pylon > Trigger
        if (hit.collider && hit.collider.CompareTag("Pylon"))
        {
            hit.collider.gameObject.SendMessage("ShutoffPylon");
        }

        else if (hit.collider && hit.collider.CompareTag("Receiver"))
        {
            hit.collider.gameObject.SendMessage("ShutoffReceiver");
        }
    }

    IEnumerator DelayedShutoff()
    {
        yield return new WaitForSeconds(1f);

        LR.positionCount = 0;
        Shutoff();
        yield break;
    }
    #endregion
}
