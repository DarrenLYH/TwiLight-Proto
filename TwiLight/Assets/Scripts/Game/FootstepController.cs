using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for Player's Footsteps Audio

public class FootstepController : MonoBehaviour
{
    public AudioClip[] footstepSounds; //Footstep Audio Clips
    
    //Range of time between footsteps
    public float minTimeBetweenFootsteps = 0.3f; 
    public float maxTimeBetweenFootsteps = 0.6f;

    private AudioSource audioSource;
    public bool isWalking = false; //Bool to check if the player is walking
    private float timeSinceLastFootstep;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Check if player is walking
        if (isWalking)
        {
            //Check if enough time has passed to play the next footstep sound
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                //Play a random footstep sound
                AudioClip footstepSound = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.PlayOneShot(footstepSound);

                timeSinceLastFootstep = Time.time; //Uodate Timer
            }
        }
    }
}
