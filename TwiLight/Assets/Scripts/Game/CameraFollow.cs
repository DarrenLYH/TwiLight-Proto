using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script to control & limit Camera Movement

public class CameraFollow : MonoBehaviour
{
    public Transform target; //Target to follow on screen
    Vector3 velocity = Vector3.zero;

    [Range(0f, 1f)]
    public float smoothTime;    

    //Camera Offset & Limits
    public Vector3 positionOffset;
    public Vector2 xLimit;
    public Vector2 yLimit;

    private void Start()
    {
        //Find & Assign the player as target if possible
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }

        else
        {
            target = null;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            //Pan camera smoothly
            Vector3 targetPosition = target.position + positionOffset;
            targetPosition = new Vector3(Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y), Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y), -10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
