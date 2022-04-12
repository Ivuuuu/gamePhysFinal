using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    [HideInInspector]
    public Vector3 StartingPosition;


    private const float maxCameraX = 13;
    private const float minCameraX = 0;
    [HideInInspector]
    public bool IsFollowing;
    [HideInInspector]
    public Transform BirdToFollow;
    void Start()
    {
        StartingPosition = transform.position;
    }

    
    void Update()
    {
        if (IsFollowing)
        {
            if (BirdToFollow != null) 
            {
                var birdPosition = BirdToFollow.transform.position;
                float x = Mathf.Clamp(birdPosition.x, minCameraX, maxCameraX);
                transform.position = new Vector3(x, StartingPosition.y, StartingPosition.z);
            }
            else
                IsFollowing = false;
        }
    }
   
}
