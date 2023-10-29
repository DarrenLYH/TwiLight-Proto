using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    public GameObject[] itemDisplay;
    public GameObject invBackground;
    public Sprite[] bagSprites; 
    bool isOpen = false;
    public int itemsHeld = 0;

    private void Update()
    {
        //Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioController.instance.PlaySFX("invOpen", 0.5f);
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen; //Update State

        //Update Display
        if (isOpen)
        {
            GetComponent<Image>().sprite = bagSprites[1];
            invBackground.transform.localScale = new Vector3(2f, 3.74f, 0);
        }

        else
        {
            GetComponent<Image>().sprite = bagSprites[0];
            invBackground.transform.localScale = new Vector3(2f, 0, 0);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isOpen);
        }
    }

    public bool AddItem(PickupObject pickup)
    {
        itemsHeld++;
        if(itemsHeld < 6)
        {
            PickupObject refItem = itemDisplay[itemsHeld - 1].GetComponent<PickupObject>();
            refItem.itemName = pickup.itemName;
            refItem.itemImage = pickup.itemImage;
            refItem.InstantiateObject();
            refItem.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            if(!isOpen)
            {
                refItem.gameObject.SetActive(false);
            }

            return true;
        }

        else
        {
            Debug.Log("I can't hold so many items!");
            return false;
        }
    }

    public bool CheckItem(string name)
    {
        foreach(GameObject item in itemDisplay)
        {
            if (item.GetComponent<PickupObject>().itemName == name)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveItem(string name)
    {
        //int itemID = 0;
        bool updateList = false;

        for (int i = 0; i < itemDisplay.Count(); i++)
        {
            PickupObject refItem = itemDisplay[i].GetComponent<PickupObject>();

            if (refItem.itemName == name)
            {
                itemsHeld--;
                refItem.itemName = null;
                refItem.itemImage = null;
                refItem.InstantiateObject();
                refItem.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                updateList = true;
                Debug.Log("found it");
            }

            if (updateList && i == itemDisplay.Count() - 1)
            {
                refItem.itemName = null;
                refItem.itemImage = null;
                refItem.InstantiateObject();
                refItem.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                Debug.Log("end");
            }

            else if(updateList)
            {
                refItem.itemName = itemDisplay[i + 1].GetComponent<PickupObject>().itemName;
                refItem.itemImage = itemDisplay[i + 1].GetComponent<PickupObject>().itemImage;
                refItem.InstantiateObject();
                refItem.gameObject.GetComponent<Image>().color = itemDisplay[i + 1].GetComponent<Image>().color;
                Debug.Log("replaced");
            }
        }

        //updateList = false;
    }
}
