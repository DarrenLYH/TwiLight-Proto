using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalPylon : MonoBehaviour
{
    //Beam Properties
    [SerializeField] private LayerMask contactCheck;
    [SerializeField] private LayerMask activeEntityCheck;
    public LineRenderer LR;
    public int beamDirection;
    Vector2 direction;
    bool isActive;
    
    public GameObject[] positions;
    int currentPos = 0;
    Vector3 destination;
    bool cycleForward = true;

    bool isInteractible;
    public bool isMovable;
    public bool isMoving = false;
    bool isTouching;

    private void Update()
    {
        if(isTouching && isMovable && !isMoving && !isActive)
        {
            isInteractible = true;
        }

        else
        {
            isInteractible = false;
        }

        if(isInteractible && Input.GetKeyDown(KeyCode.E))
        {
            MovePylon();
            GameController.instance.HideInteractPrompt();
        }
    }

    private void FixedUpdate()
    {
        if(isMovable && isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
        }
       
        if(transform.position == destination)
        {
            isMoving = false;
        }
    }

    public void MovePylon()
    {
        if (currentPos < positions.Count() - 1 && cycleForward)
        {
            currentPos++;
            destination = positions[currentPos].transform.position;
        }

        else if (currentPos == positions.Count() - 1)
        {
            cycleForward = false;
            currentPos--;
            destination = positions[currentPos].transform.position;
        }

        else if (currentPos > 0 && !cycleForward)
        {
            currentPos--;
            destination = positions[currentPos].transform.position;
        }

        else if (currentPos == 0 && !cycleForward)
        {
            cycleForward = true;
            currentPos++;
            destination = positions[currentPos].transform.position;
        }

        isMoving = true;
    }

    public void TriggerPylon()
    {
        if (!isMoving)
        {
            Invoke("EmitPylonBeam", 0.5f);
        }
    }

    public void ShutoffPylon()
    {
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(0.5f);

        LR.positionCount = 0;
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 10f, activeEntityCheck);

        //If Colliding with Pylon > Trigger
        if (hit.collider && hit.collider.CompareTag("Pylon"))
        {
            hit.collider.gameObject.SendMessage("ShutoffPylon");
        }

        else if (hit.collider && hit.collider.CompareTag("Receiver"))
        {
            hit.collider.gameObject.SendMessage("ShutoffReceiver");
        }

        int newLayer = LayerMask.NameToLayer("Pylon");
        gameObject.layer = newLayer;
        isActive = false;
        yield break;
    }

    public void EmitPylonBeam()
    {
        //Update Pylon State
        GameController.instance.HideInteractPrompt();
        isActive = true;
        int newLayer = LayerMask.NameToLayer("PylonActivated");
        gameObject.layer = newLayer;

        //Generate Beam
        LR.positionCount = 2;
        switch (beamDirection)
        {
            //Beam Up
            case 1:
                direction = Vector2.up;
                break;


            //Beam Down
            case 2:
                direction = Vector2.down;
                break;

            //Beam Left
            case 3:
                direction = Vector2.left;
                break;

            //Beam Right
            case 4:
                direction = Vector2.right;
                break;

            default:
                break;
        }

        //Raycast in the Beam Direction to Check Collision
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 10f, contactCheck);

        //If Colliding with Pylon > Trigger
        if (hit.collider && hit.collider.CompareTag("Pylon"))
        {
            LR.SetPosition(1, LR.transform.InverseTransformPoint(hit.point));
            hit.collider.gameObject.SendMessage("TriggerPylon");
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

        else
        {
            Debug.Log("nothing here");
        }
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        isTouching = true;
        if (isMovable && !isMoving && !isActive)
        {
            GameController.instance.DisplayInteractPrompt();
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        //if (isMovable && !isMoving && !isActive)
        {
            GameController.instance.HideInteractPrompt();
        }
    }
    #endregion
}
