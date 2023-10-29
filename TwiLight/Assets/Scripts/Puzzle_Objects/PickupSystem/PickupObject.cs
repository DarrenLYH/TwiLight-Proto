using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using TMPro;
//using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{
    //Reference to Player Inventory
    protected InventoryScript playerInventory;

    //Item Values
    public string itemName;
    public Sprite itemImage;
    bool isTouching = false;

    //temp
    private void Start()
    {
        playerInventory = GameController.instance.GetComponentInChildren<InventoryScript>();
        //InstantiateObject();
    }

    private void Update()
    {
        //Pickup Ability
        if (isTouching && Input.GetKeyDown(KeyCode.E))
        {
            //Play SFX
            AudioController.instance.PlaySFX("pickup", 0.05f);
            DoObjectEffect();
        }
    }

    public void InstantiateObject()
    {
        gameObject.name = itemName;
        gameObject.GetComponent<Image>().sprite = itemImage;
    }

    public virtual void DoObjectEffect() 
    {

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
