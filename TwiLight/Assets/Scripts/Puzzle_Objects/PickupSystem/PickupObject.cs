using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using TMPro;
//using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

//Parent Class for Objects that can be picked up

public class PickupObject : MonoBehaviour
{
    //Reference to Player Inventory
    protected InventoryScript playerInventory;

    //Item Values
    public string itemName;
    public Sprite itemImage;
    public bool isTouching = false;

    //temp
    private void Start()
    {
        playerInventory = GameController.instance.GetComponentInChildren<InventoryScript>();
        //InstantiateObject();
    }

    public void Update()
    {
        //Pickup Ability
        if (isTouching && Input.GetKeyDown(KeyCode.E))
        {
            DoObjectEffect();
        }
    }

    //Update Item Info
    public void InstantiateObject()
    {
        gameObject.name = itemName;
        gameObject.GetComponent<Image>().sprite = itemImage;
    }

    public virtual void DoObjectEffect() 
    {
        //Method for specific effects
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        isTouching = true;
        GameController.instance.DisplayPickupPrompt();
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        GameController.instance.HidePickupPrompt();
    }
    #endregion
}
