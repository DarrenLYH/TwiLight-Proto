using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : PickupObject
{
    public override void DoObjectEffect()
    { 
        //Increase Player LL & Start Dialogue
        Debug.Log("oh hai");
        GameController.instance.PlayerLevelUp();
        GameController.instance.DisplayHeldItem();
        GameController.instance.DC.StartDialogue("03", "0301");
        
        if (GameController.instance.GetPlayerLevel() == 3)
        {
            //GameController.instance.ToggleEndScreen();
        }

        Destroy(this.gameObject);
    }
}
