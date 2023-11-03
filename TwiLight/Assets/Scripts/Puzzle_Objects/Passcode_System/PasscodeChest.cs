using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//Specific Function for Chests 

public class PasscodeChest : PasscodeObject
{
    public Sprite[] sprites;
    public GameObject prefab;
    public override void DoUnlockAction()
    {
        base.DoUnlockAction();
        AudioController.instance.PlaySFX("chestOpen", 0.5f);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprites[1];
        Instantiate(prefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
    }
}
