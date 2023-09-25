using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;

public class PickupObject: MonoBehaviour
{
    bool isTouching = false;

    //temp
    private void Awake()
    {
        InstantiateObject();
    }

    private void Update()
    {
        //Pickup Ability
        if (isTouching && Input.GetKeyDown(KeyCode.E))
        {
            DoObjectEffect();
            Destroy(this.gameObject);
        }
    }

    public void InstantiateObject()
    {

    }

    public void DoObjectEffect() //Temporarily its just the level up
    {
        Debug.Log("oh hai");
        GameController.instance.PlayerLevelUp();
        GameController.instance.DisplayHeldItem();

        if(GameController.instance.GetPlayerLevel() == 3)
        {
            GameController.instance.ToggleEndScreen();
        }
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
