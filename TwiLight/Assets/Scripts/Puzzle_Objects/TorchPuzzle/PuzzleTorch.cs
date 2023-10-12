using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTorch : MonoBehaviour
{
    public GameObject flame;
    public bool isLit = false;
    public bool isTouching;
    public bool isInteractible;

    public void Awake()
    {
        if (isLit)
        {
            flame.SetActive(true);
        }
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isTouching && isInteractible)
        {
            ToggleTorch();
            GameController.instance.HideInteractPrompt();
            GetComponentInParent<TorchPuzzleScript>().CheckPuzzleStatus();
        }
    }

    public void ToggleTorch()
    {
        if (isLit)
        {
            isLit = false;
            flame.SetActive(false);
        }

        else
        {
            isLit = true;
            flame.SetActive(true);
        }
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInteractible)
        {
            isTouching = true;
            GameController.instance.DisplayInteractPrompt();
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (isInteractible)
        {
            isTouching = false;
            GameController.instance.HideInteractPrompt();
        }
    }
    #endregion
}
