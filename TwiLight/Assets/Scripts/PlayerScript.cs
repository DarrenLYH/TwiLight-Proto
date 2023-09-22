using System.Collections;
using System.Collections.Generic;
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
    public int lightLevel;//Level of Magical Light

    #region Player Movement
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Toggle Flashlight
        if (Time.timeScale == 1)
        {
            Look();

            if (lightLevel != 0 && Input.GetMouseButtonDown(0))
            {
                ML.isToggled = !ML.isToggled;
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
    #endregion
}
