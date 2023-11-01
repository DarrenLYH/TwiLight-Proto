using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalPylon : MonoBehaviour
{
    //Pylon Properties
    public bool isInteractible;
    public bool isBroken = false;
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

    //System Variables
    CrystalPuzzleManager PM;
    public CrystalPylon lastHitPylon;
    bool isActive; //if the beam is currently on

    private void Awake()
    {
        PM = GetComponentInParent<CrystalPuzzleManager>();
        UpdatePylonState();
    }

    private void Update()
    {
        #region Interactibility Check

        if (isMovable && isTouching && !isMoving && !isActive)
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

        if (isBroken)
        {
            if(isInteractible && Input.GetKeyDown(KeyCode.E))
            {
                FixPylon();
            }
        }

        else 
        {
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
    }

    private void FixedUpdate()
    {
        if(isMovable && isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, 0.05f);
        }
       
        if(isMoving && transform.position == destination)
        {
            PM.UpdatePuzzle();
            isMoving = false;
        }
    }

    public void UpdatePylonState()
    {
        if (isBroken)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pylonStates[4];
        }

        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pylonStates[beamDirection];
        }
    }

    #region Special Functions

    public void FixPylon()
    {
        Debug.Log("hello");

        if (GameController.instance.INV.CheckItem("crystalball"))
        {
            Debug.Log("fug");
            GameController.instance.INV.RemoveItem("crystalball");
            isBroken = false;
            UpdatePylonState();
        }

        else
        {
            Debug.Log("nah");
            GameController.instance.DC.StartDialogue("999","prompt");
        }
    }

    public void MovePylon() //Note to Self: Diagonal Movement is OK
    {
        isMoving = true; //Update Pylon Statew 

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
        UpdatePylonState();
    }
    #endregion

    #region Lazer Functions
    public void TriggerPylon()
    {
        //if (!isMoving)
        //{
        Invoke("EmitPylonBeam", 1f);
        //EmitPylonBeam();
        Debug.Log("pylon triggered");
        //}
    }

    public void EmitPylonBeam()
    {
        if (!isBroken)
        {
            //Update Pylon State
            GameController.instance.HideInteractPrompt();
            isActive = true;

            //Redundant
            //int newLayer = LayerMask.NameToLayer("PylonActivated");
            //gameObject.layer = newLayer;

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

                else if(hit.collider != lastHitPylon)
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
                if(lastHitPylon != null)
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
    }

    public void Recalculate()
    {
        if (isActive)
        {
            Debug.Log("pylon Recalculate...");
            EmitPylonBeam();
        }
    }

    public void ShutoffPylon()
    {
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

        isActive = false;
    }
    #endregion
    
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
