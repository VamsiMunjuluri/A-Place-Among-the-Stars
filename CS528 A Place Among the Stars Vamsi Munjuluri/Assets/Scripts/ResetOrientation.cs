using UnityEngine;

public class ResetOrientation : MonoBehaviour
{
    public Transform objectToReset; // Drag your camera or player here in the Inspector

    // Default orientation. Adjust these values to your desired default orientation
    private Quaternion initialRotation = Quaternion.Euler(0, -180, 0);

    // Determines if we are currently resetting the orientation
    private bool isResetting = false;

    // Speed at which the rotation resets
    public float resetSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // If we are resetting, smoothly interpolate the rotation
        if (isResetting)
        {
            objectToReset.rotation = Quaternion.Slerp(objectToReset.rotation, initialRotation, Time.deltaTime * resetSpeed);

            // Optionally, you could stop resetting once close enough to the target rotation
            if (Quaternion.Angle(objectToReset.rotation, initialRotation) < 0.1f)
            {
                objectToReset.rotation = initialRotation; // Snap to final rotation
                isResetting = false; // Stop the resetting process
            }
        }
    }

    // Method to start the smooth reset process
    public void StartResetToDefaultOrientation()
    {
        isResetting = true;
    }
}
