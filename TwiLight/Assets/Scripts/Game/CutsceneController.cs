using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controller for cutscenes and transition animation

public class CutsceneController : MonoBehaviour
{
    public SceneController SC;
    public Animator fadeAnimator;        //Transition Animator Componenet
    public GameObject[] storedCutscenes; //Cutscenes to Display
    public string[] storedSFX;           //SFX to Play
    int i = 0; //indexer

    private void Start()
    {
        ProgressCutscene(); //Start playing cutscenes
    }

    private void Update()
    {
        //Move to next cutscene on click
        if(Input.GetMouseButtonDown(0))
        {
            ProgressCutscene();
        }
    }

    public void ProgressCutscene()
    {
        if(i == storedCutscenes.Length)
        {
            StartCoroutine(Transition());
        }

        else
        {
            storedCutscenes[i].SetActive(true);
            AudioController.instance.PlaySFX(storedSFX[i],0.5f);
            i++;
        }
    }
    
    public IEnumerator Transition()
    {
        fadeAnimator.SetBool("isOver",true);
        yield return new WaitForSeconds(5f);
        Debug.Log("scene unloaded");
        SC.ToScene("FullLevelM2");
    }
}
