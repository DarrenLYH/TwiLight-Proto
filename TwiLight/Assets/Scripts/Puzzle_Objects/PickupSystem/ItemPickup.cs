using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class ItemPickup : PickupObject
{
    GameObject prefabUI;

    public override void DoObjectEffect()
    {
        if (playerInventory.AddItem(this))
        {
            Destroy(this.gameObject);
        }
    }
}
