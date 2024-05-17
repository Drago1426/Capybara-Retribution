using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPos;
    private float parallaxEffect = 1; // Factor by which the parallax effect is scaled

    private Transform cameraTransform;

    void Start()
    {
        // Initialize positions and lengths based on the sprite size
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // Find the main camera in the scene and store its transform
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Calculate the movement of the background based on camera movement
        float temp = (cameraTransform.position.x * (1 - parallaxEffect));
        float dist = (cameraTransform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        // Adjust start position if the camera moves beyond the current background bounds
        if (temp > startPos + length) 
        {
            startPos += length;
        }
        else if (temp < startPos - length) 
        {
            startPos -= length;
        }
    }
}
