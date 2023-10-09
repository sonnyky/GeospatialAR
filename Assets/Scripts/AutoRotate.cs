using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        // Calculate rotation for this frame
        float rotationThisFrame = rotationSpeed * Time.deltaTime;

        // Rotate around the Y axis
        transform.Rotate(0, 0, rotationThisFrame);
    }
}
