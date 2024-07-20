using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        // Cache the main camera reference
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Ensure the sprite is always facing the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}