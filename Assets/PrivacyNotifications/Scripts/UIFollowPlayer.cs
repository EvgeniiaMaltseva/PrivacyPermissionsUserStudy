using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public GameObject uiElement;     // The UI element (book)
    public Transform xrCamera;       // The XR Camera (headset)
    public float distance = 2.0f;    // Distance from the player
    public float heightOffset = 0.5f; // Vertical offset from the camera position
    public float horizontalOffset = 0.0f; // Horizontal offset from the camera position

    private bool followPlayer = false; // Controls whether the UI follows the player
    private Quaternion initialLocalRotation; // To store the initial rotation relative to the UI's local space

    void Start()
    {
        // Store the initial local rotation of the UI element
        initialLocalRotation = uiElement.transform.localRotation;
        // Ensure the UI starts in its original position
        followPlayer = false;
    }

    void Update()
    {
        // If followPlayer is true, reposition the UI to follow the player
        if (followPlayer)
        {
            // Calculate the new position with horizontal and height offsets
            Vector3 forwardOffset = xrCamera.forward * distance;
            Vector3 rightOffset = xrCamera.right * horizontalOffset; // Add horizontal offset
            Vector3 newPosition = xrCamera.position + forwardOffset + rightOffset;
            newPosition.y = xrCamera.position.y + heightOffset; // Maintain vertical offset

            uiElement.transform.position = newPosition;

            // Face the player while maintaining the original tilt and roll
            Vector3 directionToPlayer = (xrCamera.position - uiElement.transform.position).normalized;
            Quaternion lookAtPlayerRotation = Quaternion.LookRotation(-directionToPlayer); // Rotate towards player

            // Combine the initial local rotation with the new rotation to preserve tilt
            uiElement.transform.rotation = lookAtPlayerRotation * Quaternion.Inverse(uiElement.transform.parent?.rotation ?? Quaternion.identity) * initialLocalRotation;
        }
    }

    // Call this method to activate UI following
    public void StartFollowingPlayer()
    {
        followPlayer = true;
    }

    // Call this method to stop UI following
    public void StopFollowingPlayer()
    {
        followPlayer = false;
    }
}
