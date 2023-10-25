using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LO_CrystalActivator : LightableObject
{
    //Object Components
    [SerializeField] private LayerMask contactCheck;
    public LineRenderer LR;
    public int beamDirection;
    Vector2 direction;

    //Object State
    public bool isToggled;
    public bool isTouching;

    private void Update()
    {
        if (isTouching && player.GetComponent<PlayerScript>().lampPlaced == false && Input.GetMouseButtonDown(1))
        {
            //ToggleActivator();
        }
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
            isTriggered = false;
            StartCoroutine(DelayedShutoff());
            Debug.Log("Crystal Deactivated");
        }
    }

    public override void OnHitEnter()
    {
        isLit = true;
        //Override Default Action
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

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLamp"))
        {
            ToggleActivator();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLamp"))
        {
            ToggleActivator();
        }
    }

    /*  void OnCollisionEnter2D(Collision2D collision)
      {
          PlayerScript PS = collision.gameObject.GetComponent<PlayerScript>();
          if (collision.gameObject.CompareTag("Player") && PS.currentLight == 3)
          {
              isTouching = true;
              PS.lampPlaceable = true;
          }
      }

      void OnCollisionExit2D(Collision2D collision)
      {
          if (collision.gameObject.CompareTag("Player"))
          {
              isTouching = false;
              collision.gameObject.GetComponent<PlayerScript>().lampPlaceable = false;
          }
      }*/
    #endregion
}