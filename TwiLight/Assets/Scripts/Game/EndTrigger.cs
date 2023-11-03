using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioController.instance.PlaySFX("levelup", 1f);
        GameController.instance.Invoke("ToggleEndScreen", 1f);
    }
}
