using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    public float maxScroll = 80f;
    public float minScroll = 10f;

    private Camera cam;
    private float previousScrollState = 0;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y * scrollSpeed;
        cam.fieldOfView -= scroll;
        if (cam.fieldOfView > maxScroll)
            cam.fieldOfView = maxScroll;
        else if (cam.fieldOfView < minScroll)
            cam.fieldOfView = minScroll;
    }
}
