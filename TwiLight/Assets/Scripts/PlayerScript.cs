using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private MagicalLight ML;
    public float moveSpeed = 5f;
    public Rigidbody2D RB;
    Vector2 movement;
    public int lightLevel = 1;

    #region Player Movement
    void Update()
    {
        Look();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Toggle Flashlight
        if (Input.GetMouseButtonDown(0))
        {
            ML.isToggled = !ML.isToggled;
        }
    }

    void FixedUpdate()
    {
        RB.MovePosition(RB.position + movement * moveSpeed * Time.fixedDeltaTime);

        ML.SetAimDirection(this.transform.up);
        ML.SetOrigin(transform.position);
    }

    void Look()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }
    #endregion
}
