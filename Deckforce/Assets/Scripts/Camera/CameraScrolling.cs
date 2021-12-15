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
        cam.orthographicSize -= scroll;
        if (cam.orthographicSize > maxScroll)
            cam.orthographicSize = maxScroll;
        else if (cam.orthographicSize < minScroll)
            cam.orthographicSize = minScroll;
    }
}
