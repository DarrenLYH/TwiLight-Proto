using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : PickupObject
{
    public override void DoObjectEffect()
    { 
        Debug.Log("oh hai");
        GameController.instance.PlayerLevelUp();
        GameController.instance.DisplayHeldItem();

        if (GameController.instance.GetPlayerLevel() == 3)
        {
            GameController.instance.ToggleEndScreen();
        }

        Destroy(this.gameObject);
    }
}
