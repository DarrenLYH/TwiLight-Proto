using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for Audio

public class AudioController : MonoBehaviour
{
    //Array to store audio files
    public Sound[] bgmLibrary, sfxLibrary;
    public AudioSource bgmSource, sfxSource;

    //Audio Controller & Properties
    public static AudioController instance;
    public static float sfxVol; //Global SFX Value
    public static float bgmVol; //Global BGM Value
    public string buttonSound;  //Default Button Sound to play
    private void Awake()
    {
        //Instantiate AudioController
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Default Volume Settings
        BGMVolume(0.05f);
        SFXVolume(0.5f);

        //Play BGM on Start
        PlayBGM("placeholder");
    }

    #region Audio Triggers / Controls
    public void PlayBGM(string name)
    {
        Sound clip = Array.Find(bgmLibrary, x => x.name == name);

        if (clip == null)
        {
            Debug.Log("Audio Not Found");
        }

        else
        {
            bgmSource.clip = clip.clip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(string name, float modifier)
    {
        Sound clip = Array.Find(sfxLibrary, x => x.name == name);

        if (clip == null)
        {
            Debug.Log("Audio Not Found");
        }

        else
        {
            sfxSource.volume = sfxVol * modifier;
            sfxSource.clip = clip.clip;
            sfxSource.Play();
        }
    }
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    //Default Button SFX Method (Can be triggered by Button)
    public void PlayButtonSFX()
    {
        PlaySFX(buttonSound, 0.05f);
    }
    #endregion

    #region Global Volume Settings (Unused)
    //Method to Adjust Global Volume Settings (Unused)
    public void BGMVolume(float volume)
    {
        bgmVol = volume;
        bgmSource.volume = bgmVol;
    }

    public void SFXVolume(float volume)
    {
        sfxVol = volume;
        sfxSource.volume = sfxVol;
    }
    #endregion
}
