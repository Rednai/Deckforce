using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{


    public float dragSpeed = 2;
    private Vector3 dragOrigin;


    void Update()
    {
        

        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        if (transform.position.x + move.x > -1 | transform.position.x + move.x < -37)
            return;
        if (transform.position.y + move.y > 30 | transform.position.y + move.y < 1)
            return;
        transform.Translate(move, Space.World);
    }
}
