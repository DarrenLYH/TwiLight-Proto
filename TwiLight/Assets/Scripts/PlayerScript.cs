using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //Player Components
    [SerializeField] private MagicalLight ML;
    public FootstepController FC;
    public Rigidbody2D RB;
    public GameObject eyes;
    public GameObject glow;
    
    public Animator animator;
    public Animator levelupAnimator;

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

        if (movement != Vector2.zero)
        {
            FC.isWalking = true;
        }

        else
        {
            FC.isWalking = false;
        }
        #endregion

        //Toggle Flashlight
        if (Time.timeScale == 1)
        {
            Look();

            //SFX
            if (currentLight == 1 && Input.GetMouseButtonDown(0))
            {
                AudioController.instance.PlaySFX("candle", 0.5f);
            }

            if (currentLight == 2 && Input.GetMouseButtonDown(0))
            {
                AudioController.instance.PlaySFX("torch", 0.5f);
            }

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
            if (Input.GetMouseButtonDown(0))
            {
                if (currentLight == 3 && lampPlaceable && !lampPlaced)
                {
                    AudioController.instance.PlaySFX("lamp", 0.75f);
                    animator.SetInteger("CurrentLight", 0);
                    PlaceLight();
                }

                else if (currentLight == 3 && lampContacted)
                {
                    AudioController.instance.PlaySFX("pickup", 0.05f);
                    animator.SetInteger("CurrentLight", 3);
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
        if (lightLevel != 0)
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
        if (lightLevel != 0)
        {
            currentLight++;

            if (currentLight > lightLevel)
            {
                currentLight = 1;
            }

            animator.SetInteger("CurrentLight", currentLight);
            GameController.instance.DisplayHeldItem();
            AudioController.instance.PlaySFX("invOpen", 0.1f);
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
