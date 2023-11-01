using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CrystalActivator : MonoBehaviour
{
    //Object Components
    [SerializeField] private LayerMask contactCheck;
    public LineRenderer LR;
    public int beamDirection;
    Vector2 direction;

    //Object State
    public bool isActive;
    public bool isToggled;

    CrystalPuzzleManager PM;
    public CrystalPylon lastHitPylon;

    private void Awake()
    {
        PM = GetComponentInParent<CrystalPuzzleManager>();
    }

    public void ToggleActivator()
    {
        isToggled = !isToggled;
        
        if (isToggled)
        {
            EmitBeam();
            Debug.Log("Crystal Activated");
        }

        else
        {
            isActive = false;
            Shutoff();
            Debug.Log("Crystal Deactivated");
        }
    }

    #region Beam Emitter
    public void EmitBeam()
    {
        isActive = true;
        LR.positionCount = 2;

        switch (beamDirection)
        {
            //Beam Up
            case 0:
                direction = Vector2.up;
                break;
                

            //Beam Down
            case 1:
                direction = Vector2.right;
                break;

            //Beam Left
            case 2:
                direction = Vector2.down;
                break;

            //Beam Right
            case 3:
                direction = Vector2.left;
                break;

            default:
                break;
        }
                
        //Raycast in the Beam Direction to Check Collision
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 20f, contactCheck);

        //If Colliding with Pylon > Trigger
        if (hit.collider && hit.collider.CompareTag("Pylon"))
        {
            if (lastHitPylon == null)
            {
                //Set Last Hit Pylon
                lastHitPylon = hit.collider.GetComponent<CrystalPylon>();

                LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
                hit.collider.gameObject.SendMessage("TriggerPylon");
            }

            else if (hit.collider != lastHitPylon)
            {
                lastHitPylon.ShutoffPylon();

                lastHitPylon = hit.collider.GetComponent<CrystalPylon>();

                LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
                hit.collider.gameObject.SendMessage("TriggerPylon");
            }

            else
            {
                LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
                hit.collider.gameObject.SendMessage("TriggerPylon");
            }
        }

        //If Colliding with Receiver > Trigger
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

        else if (hit.collider && hit.collider.CompareTag("SpecialObject"))
        {
            if (lastHitPylon != null)
            {
                lastHitPylon.ShutoffPylon();
                lastHitPylon = null;
            }

            LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
        }

        else
        {
            Debug.Log("nothing here");
        }
    }

    public void Recalculate()
    {
        if (isActive)
        {
            EmitBeam();
        }
    }

    public void Shutoff()
    {
        LR.positionCount = 0;

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
    #endregion

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLamp")) //If lamp is placed
        {
            ToggleActivator();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLamp")) //If lamp is removed
        {
            ToggleActivator();
        }
    }
    #endregion
}
