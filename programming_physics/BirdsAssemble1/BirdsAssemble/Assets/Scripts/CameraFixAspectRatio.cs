using UnityEngine;
using System.Collections;

public class CameraFixAspectRatio : MonoBehaviour
{

    
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        float aspect = Mathf.Round(camera.aspect * 100f) / 100f;

        if (aspect == 0.6f) 
            camera.orthographicSize = 5;
        else if (aspect == 0.56f) 
        {
            camera.orthographicSize = 4.6f;
        }

    }
}
