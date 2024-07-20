using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    private float zoom;
    private float zoomMultiplier = 4;
    private float minZoom = 1f;
    public float maxZoom = 3f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    [SerializeField] private Camera cam;
    private void Start()
    {
        zoom = cam.orthographicSize;
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
    }

}
