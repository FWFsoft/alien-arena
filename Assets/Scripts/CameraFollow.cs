using Unity.VisualScripting;

using UnityEngine;

using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private InputHandler inputHandler;
    public Transform target; // The target the camera should follow (typically your player character)
    public Vector3 offset = new Vector3(0f, 0f, -30f); // Offset from the target position. Adjust if needed.

    public float mDelta = 10; // Pixels. The width border at the edge in which the movement work
    public float mSpeed = 3.0f; // Scale. Speed of the movement

    private bool followTarget = true;

    void LateUpdate()
    {
        // Check if there's a target
        // Based this on if you hit the spacebar
        if (followTarget || inputHandler.isScreenLocked)
        {
            setCameraToPlayer();
        }
        else
        {
            unlockCameraMovementAsAppropriate();
        }
        if (inputHandler.toggleScreenLock)
        {
            followTarget = !followTarget;
        }
    }

    void setCameraToPlayer()
    {

        if (target != null)
        {
            // Update the camera's position to follow the target, plus the offset
            transform.position = target.position + offset;
        }
    }

    void unlockCameraMovementAsAppropriate()
    {
        if (Input.mousePosition.x >= Screen.width - mDelta)
        {
            // Move the camera
            transform.position += mSpeed * Time.deltaTime * Vector3.right;
        }
        else if (Input.mousePosition.x <= mDelta)
        {
            transform.position += mSpeed * Time.deltaTime * Vector3.left;
        }
        else if (Input.mousePosition.y >= Screen.height - mDelta)
        {
            transform.position += mSpeed * Time.deltaTime * Vector3.up;
        }
        else if (Input.mousePosition.y <= mDelta)
        {
            transform.position += mSpeed * Time.deltaTime * Vector3.down;
        }
    }
}
