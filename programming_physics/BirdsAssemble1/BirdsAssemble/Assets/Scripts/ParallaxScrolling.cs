using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {
    public float ParallaxFactor;
    void Start () {
        camera = Camera.main;
        previousCameraTransform = camera.transform.position;
	}

    Camera camera;
	
	void Update () {
        Vector3 delta = camera.transform.position - previousCameraTransform;
        delta.y = 0; delta.z = 0;
        transform.position += delta / ParallaxFactor;


        previousCameraTransform = camera.transform.position;
	}

   
    Vector3 previousCameraTransform;
}
