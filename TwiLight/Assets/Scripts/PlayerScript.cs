using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Player Components
    [SerializeField] private MagicalLight ML;
    public Rigidbody2D RB;
    public GameObject eyes;
    public GameObject glow;
    public Animator animator;
    
    //Player Variables
    Vector2 movement;
    public float moveSpeed = 5f;
    public int lightLevel;   //Maximum Level of Magical Light
    public int currentLight; //Currently Held Light

    public GameObject prefabLamp;
    public GameObject placedLamp;
    public bool lampPlaceable = false;
    public bool lampPlaced = false;
    public bool lampContacted = false;

    void Update()
    {
        #region Movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        #endregion
        
        //Toggle Flashlight
        if (Time.timeScale == 1)
        {
            Look();

            //Toggle Light On/Offs
            if (lightLevel != 0 && currentLight != 3 && Input.GetMouseButton(0))
            {
                ML.isToggled = true;
            }

            else
            {
                ML.isToggled = false;
            }

            //Place Lamp
            if (Input.GetMouseButtonDown(1))
            {
                if (currentLight == 3 && lampPlaceable && !lampPlaced)
                {
                    PlaceLight();
                }

                else if (lampContacted)
                {
                    PickupLight();
                }
            }

            //Switch between held Light sources
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchLight();
            }
        }

        //Activate Glow when light is picked up
        if(lightLevel != 0)
        {
            glow.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        //Move Player
        RB.MovePosition(RB.position + movement * moveSpeed * Time.fixedDeltaTime);

        //Player Look Direction
        ML.SetAimDirection(eyes.transform.up);
        ML.SetOrigin(transform.position);
    }

    //Make Player look toward mouse cursor (For Light)
    void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        eyes.transform.up = direction;      
    }

    public void SwitchLight()
    {
        if(lightLevel != 0)
        {
            currentLight++;

            if(currentLight > lightLevel)
            {
                currentLight = 1;
            }

            GameController.instance.DisplayHeldItem();
        }
    }

    public void PlaceLight()
    {
        lampPlaceable = false;
        lampPlaced = true;
        Instantiate(prefabLamp, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
    }

    public void PickupLight()
    {
        lampPlaceable = true;
        lampPlaced = false;
        Destroy(placedLamp);
    }
}
