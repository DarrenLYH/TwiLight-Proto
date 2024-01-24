using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UNUSED CODE
public class MagicalLight_Lamp : MonoBehaviour
{
    PlayerScript PS;

    void Start()
    {
        PS = GameController.instance.PS;
    }

    #region Contact Check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PS.lampContacted = true;
            //PS.placedLamp = this.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PS.lampContacted = false;
        }
    }
    #endregion
}
