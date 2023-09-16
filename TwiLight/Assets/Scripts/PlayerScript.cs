using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private MagicalLight ML;
    public Rigidbody2D RB;
    Vector2 movement;
    public float moveSpeed = 5f;
    public int lightLevel = 1;//Level of Magical Light

    #region Player Movement
    void Update()
    {
        Look();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Toggle Flashlight
        if (Time.timeScale == 1 && Input.GetMouseButtonDown(0))
        {
            ML.isToggled = !ML.isToggled;
        }
    }

    void FixedUpdate()
    {
        //Move Player
        RB.MovePosition(RB.position + movement * moveSpeed * Time.fixedDeltaTime);

        //Player Look Direction
        ML.SetAimDirection(this.transform.up);
        ML.SetOrigin(transform.position);
    }

    //Make Player look toward mouse cursor (For Light)
    void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;      
    }
    #endregion
}
