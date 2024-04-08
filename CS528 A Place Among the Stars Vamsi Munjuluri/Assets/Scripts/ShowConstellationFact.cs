using UnityEngine;

public class ShowConstellationFact : MonoBehaviour
{
    public Transform objectToReset; // Drag your camera or player here in the Inspector

    // Default position and rotation. Adjust these values to your desired default values
    private Vector3 initialPosition = Vector3.zero; // This will set the position to the origin
    private Quaternion initialRotation = Quaternion.Euler(-18, 54, -48); // Desired default orientation

    // Determines if we are currently resetting the position and orientation
    private bool isResetting = false;

    // Speed at which the position and rotation resets
    public float resetSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // If we are resetting, smoothly interpolate the position and rotation
        if (isResetting)
        {
            // Smoothly interpolate the position
            objectToReset.position = Vector3.Lerp(objectToReset.position, initialPosition, Time.deltaTime * resetSpeed);

            // Smoothly interpolate the rotation
            objectToReset.rotation = Quaternion.Slerp(objectToReset.rotation, initialRotation, Time.deltaTime * resetSpeed);

            // Optionally, stop resetting once close enough to the target position and rotation
            if (Vector3.Distance(objectToReset.position, initialPosition) < 0.1f &&
                Quaternion.Angle(objectToReset.rotation, initialRotation) < 0.1f)
            {
                objectToReset.position = initialPosition; // Snap to final position
                objectToReset.rotation = initialRotation; // Snap to final rotation
                isResetting = false; // Stop the resetting process
            }
        }
    }

    // Method to start the smooth reset process
    public void StartResetToDefault()
    {
        isResetting = true;
    }
}
