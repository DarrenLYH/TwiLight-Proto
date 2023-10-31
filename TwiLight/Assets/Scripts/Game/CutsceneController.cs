using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public SceneController SC;
    public Animator fadeAnimator;
    public GameObject[] storedCutscenes;
    public string[] storedSFX;
    int i = 0;

    private void Start()
    {
        ProgressCutscene();
    }

    private void Update()
    {
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
