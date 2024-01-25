using System.Collections;
using UnityEngine;

public class StreetMarker : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 30.0f;

    // Update is called once per frame
    void Update()
    {
        // Calculate rotation for this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Rotate around the Y axis
        transform.Rotate(0, 0, rotationThisFrame);

    }
}
