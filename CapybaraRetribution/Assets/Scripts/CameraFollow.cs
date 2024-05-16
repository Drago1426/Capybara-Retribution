using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera will follow
    [SerializeField]
    private Transform target;

    // The speed with which the camera will follow the target
    [SerializeField]
    private float smoothSpeed = 0.125f;

    // The offset of the camera from the target
    [SerializeField]
    private Vector3 offset;

    void FixedUpdate()
    {
        if (target == null)
            return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothed position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera position to the smoothed position
        transform.position = smoothedPosition;
    }
}
