using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pickup to Upgrade Player Level

public class LightPickup : PickupObject
{
    public string cutsceneID;
    public string lineID;

    public override void DoObjectEffect()
    {
        AudioController.instance.PlaySFX("levelup", 1f);

        //Increase Player LL & Start Dialogue
        GameController.instance.PlayerLevelUp();
        GameController.instance.DisplayHeldItem();
        GameController.instance.DC.StartDialogue(cutsceneID, lineID);

        Destroy(this.gameObject);
    }
}
