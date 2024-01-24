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
    public bool isTouching;

    PlayerScript PS;
    public GameObject lampSprite;
    //CrystalPuzzleManager PM;
    public CrystalPylon lastHitPylon;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PS = player.GetComponent<PlayerScript>();
        //PM = GetComponentInParent<CrystalPuzzleManager>();
    }

    private void Update()
    {
        if (isTouching && Input.GetKeyDown(KeyCode.E))
        {
            if (PS.lampPlaceable)
            {
                PS.PlaceLamp();
                PS.lampContacted = true;
                ToggleActivator();
            }

            else if (isActive)
            {
                PS.PickupLamp();
                PS.lampContacted = false;
                isTouching = false;
                ToggleActivator();
            }
        }
    }

    public void ToggleActivator()
    {
        isToggled = !isToggled;

        if (isToggled)
        {
            lampSprite.SetActive(true);
            isActive = true;
            EmitBeam();
            Debug.Log("Crystal Activated");
        }

        else
        {
            lampSprite.SetActive(false);
            isActive = false;
            Shutoff();
            Debug.Log("Crystal Deactivated");
        }
    }

    #region Beam Emitter
    public void EmitBeam()
    {
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
        if (isActive) //Check if Activator is active == Lamp is here
        {
            PS.lampContacted = true;
            isTouching = true;
            GameController.instance.DisplayInteractPrompt();
        }

        if (PS.lampPlaceable && PS.currentLight == 3) //Check if Player has Lamp
        {
            isTouching = true;
            GameController.instance.DisplayInteractPrompt();
        }
    }

    void OnCollisionExit2D(Collision2D collision) //Reset Variables
    {
        isTouching = false;
        GameController.instance.HideInteractPrompt();
        PS.lampContacted = false;
    }

    /* UNUSED CODE
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
    }*/
    #endregion
}
