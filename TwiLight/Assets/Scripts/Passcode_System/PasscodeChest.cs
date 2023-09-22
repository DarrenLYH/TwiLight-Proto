using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PasscodeChest : PasscodeObject
{
    public GameObject prefab;
    public override void DoUnlockAction()
    {
        base.DoUnlockAction();
        Instantiate(prefab, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
    }
}
