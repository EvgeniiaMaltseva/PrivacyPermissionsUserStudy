using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    public GameObject uiElement;
    public Transform xrCamera;
    public float distance = 2.0f;    // Distance from the player
    public float heightOffset = 0.5f; // Vertical offset from the camera position
    public float horizontalOffset = 0.0f; // Horizontal offset from the camera position
    private bool followPlayer = false; 
    private Quaternion initialLocalRotation; // To store the initial rotation relative to the UI's local space

    void Start()
    {
        initialLocalRotation = uiElement.transform.localRotation;
        followPlayer = false;
    }

    void Update()
    {
        if (followPlayer)
        {
            Vector3 forwardOffset = xrCamera.forward * distance;
            Vector3 rightOffset = xrCamera.right * horizontalOffset; 
            Vector3 newPosition = xrCamera.position + forwardOffset + rightOffset;
            newPosition.y = xrCamera.position.y + heightOffset; 

            uiElement.transform.position = newPosition;

            Vector3 directionToPlayer = (xrCamera.position - uiElement.transform.position).normalized;
            Quaternion lookAtPlayerRotation = Quaternion.LookRotation(-directionToPlayer);

            // Combine the initial local rotation with the new rotation to preserve tilt
            uiElement.transform.rotation = lookAtPlayerRotation * Quaternion.Inverse(uiElement.transform.parent?.rotation ?? Quaternion.identity) * initialLocalRotation;
        }
    }

    public void StartFollowingPlayer()
    {
        followPlayer = true;
    }

    public void StopFollowingPlayer()
    {
        followPlayer = false;
    }
}
