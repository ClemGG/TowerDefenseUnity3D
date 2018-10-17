using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rot : MonoBehaviour {

    public float speed = 10f; // Haven't tested it yet

    void Update()
    {

        transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * speed * Time.deltaTime);

    }
}
