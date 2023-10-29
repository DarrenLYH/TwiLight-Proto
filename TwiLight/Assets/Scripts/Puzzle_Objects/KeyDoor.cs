using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject door;
    public string[] requiredKeys;

    bool isTouching = false;

    private void Update()
    {
        if (isTouching && Input.GetKeyDown(KeyCode.E))
        {
            CheckUnlock();
        }
    }

    public void CheckUnlock()
    {
        bool hasKeys = false;

        foreach(string key in requiredKeys)
        {
            if (GameController.instance.INV.CheckItem(key))
            {
                hasKeys = true;
            }

            else
            {
                hasKeys = false;
                break;
            }
        }

        if (hasKeys)
        {
            foreach(string key in requiredKeys)
            {
                GameController.instance.INV.RemoveItem(key);
            }

            AudioController.instance.PlaySFX("doorOpen", 1f);
            door.SetActive(false);
        }
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        isTouching = true;
        GameController.instance.DisplayInteractPrompt();
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isTouching = false;
        GameController.instance.HideInteractPrompt();
    }
    #endregion
}
