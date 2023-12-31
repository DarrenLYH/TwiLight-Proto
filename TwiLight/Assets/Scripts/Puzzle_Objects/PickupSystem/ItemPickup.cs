using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for other item

public class ItemPickup : PickupObject
{
    public override void DoObjectEffect()
    {
        AudioController.instance.PlaySFX("pickup", 0.05f);

        //Add Item to Player Inventory
        if (playerInventory.AddItem(this))
        {
            Destroy(this.gameObject);
        }
    }
}
