using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalPylon : MonoBehaviour
{
    //Pylon Properties
    public bool isInteractible;
    public bool isMovable;
    public bool isMoving = false;
    public bool isRotatable;
    bool isTouching; //Check for player contact
    public Sprite[] pylonStates;

    //Movement Variables
    public GameObject[] positions; //Movable positions
    int currentPos = 0;
    Vector3 destination;
    bool cycleForward = true;

    //Beam Variables
    [SerializeField] private LayerMask contactCheck;
    [SerializeField] private LayerMask activeEntityCheck;
    public LineRenderer LR;
    public int beamDirection;
    Vector2 direction;
    bool isActive; //if the beam is currently on

    private void Awake()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = pylonStates[beamDirection];
    }
    private void Update()
    {
        #region Interactibility Check
        if(isMovable && isTouching && !isMoving && !isActive)
        {
            isInteractible = true;
        }

        else if (isRotatable && isTouching && !isMoving && !isActive)
        {
            isInteractible = true;
        }

        else
        {
            isInteractible = false;
        }
        #endregion

        //Move Pylon
        if (isMovable && isInteractible && Input.GetKeyDown(KeyCode.E))
        {
            MovePylon();
            GameController.instance.HideInteractPrompt();
        }

        //Rotate Pylon
        if (isRotatable && isInteractible && Input.GetKeyDown(KeyCode.E))
        {
            RotatePylon();
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
        isMoving = true; //Update Pylon State

        //Cycle position forward
        if (currentPos < positions.Count() - 1 && cycleForward)
        {
            currentPos++;
            destination = positions[currentPos].transform.position;
        }

        //Change direction and cycle position backward
        else if (currentPos == positions.Count() - 1)
        {
            cycleForward = false;
            currentPos--;
            destination = positions[currentPos].transform.position;
        }

        //Cycle position backward
        else if (currentPos > 0 && !cycleForward)
        {
            currentPos--;
            destination = positions[currentPos].transform.position;
        }

        //Change direction and cycle position forward
        else if (currentPos == 0 && !cycleForward)
        {
            cycleForward = true;
            currentPos++;
            destination = positions[currentPos].transform.position;
        }
    }

    public void RotatePylon()
    {
        //Switch Beam Direction
        beamDirection += 1;

        //Loop around if out of index
        if(beamDirection > 3)
        {
            beamDirection = 0;
        }

        //Set Sprite Direction
        gameObject.GetComponent<SpriteRenderer>().sprite = pylonStates[beamDirection];
    }

    public void TriggerPylon()
    {
        if (!isMoving)
        {
            Invoke("EmitPylonBeam", 1f);
            Debug.Log("abaabababa");
        }
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

    public void ShutoffPylon()
    {
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(0.5f);

        LR.positionCount = 0;
        RaycastHit2D hit = Physics2D.Raycast(LR.transform.position, direction, 20f, activeEntityCheck);

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

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        isTouching = true;
        if (!isMoving && !isActive && isMovable)
        {
            GameController.instance.DisplayInteractPrompt();
        }

        else if (!isMoving && !isActive && isRotatable)
        {
            GameController.instance.DisplayInteractPrompt();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        GameController.instance.HideInteractPrompt();
      
    }
    #endregion
}
