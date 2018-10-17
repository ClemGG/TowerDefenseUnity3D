using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    public float normalSpeed = 10f;
    public float mouseSpeed = 10f;

    private bool isUsingMouse = false;

    // Update is called once per frame
    void Update()
    {
        if (!isUsingMouse)
            transform.Rotate(new Vector3(0, 1, 0), normalSpeed);

        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(Vector3.zero, new Vector3(0f, Input.GetAxis("Mouse X"), 0f), Time.deltaTime * mouseSpeed);
            isUsingMouse = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isUsingMouse = false;
        }
    }
}
