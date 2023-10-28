using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public SceneController SC;
    public Animator anim;
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
            anim.SetBool("isOver", true);
            Debug.Log("scene unloaded");
            SC.ToScene("Test");
        }

        else
        {
            storedCutscenes[i].SetActive(true);
            AudioController.instance.PlaySFX(storedSFX[i],1f);
            i++;
        }
    }
    
    public IEnumerator Cutscene()
    {
        //TBC
        return null;
    }
}
