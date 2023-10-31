using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTorch : MonoBehaviour
{
    public GameObject glow;
    public Animator animator;

    public bool isLit = false;
    public bool isTouching;
    public bool isInteractible;

    public void Start()
    {
        if (isInteractible)
        {
            animator.SetBool("isInteractible", true);
        }

        else
        {
            animator.SetBool("isInteractible", false);
        }

        if (isLit)
        {
            animator.SetBool("isActive", true);
            glow.SetActive(true);
        }

        else
        {
            animator.SetBool("isActive", false);
        }
        
    }
    public void Update()
    {
        //Disable Interaction if not holding the right Light
        if(isTouching && GameController.instance.GetPlayerLight() != 2)
        {
            isTouching = false;
            GameController.instance.HideInteractPrompt();
        }

        if(Input.GetKeyDown(KeyCode.E) && isTouching && isInteractible)
        {
            AudioController.instance.PlaySFX("torch", 1f);
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
            animator.SetBool("isActive", false);
            glow.SetActive(false);
        }

        else
        {
            isLit = true;
            animator.SetBool("isActive", true);
            glow.SetActive(true);
        }
    }

    #region Contact Check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.instance.GetPlayerLight() == 2 && isInteractible)
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
