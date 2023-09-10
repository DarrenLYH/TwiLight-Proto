using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LightableObject : MonoBehaviour
{
    public int levelRequirement;

    public abstract void DoInteraction();
}
