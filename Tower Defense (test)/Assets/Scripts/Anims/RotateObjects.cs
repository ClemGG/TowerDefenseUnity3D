using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjects : MonoBehaviour {

    [SerializeField] private Transform[] listOfObjectsToRotate;
    [SerializeField] private float speed;

    [Header("Rotate Along : ")]

    [SerializeField] bool X;
    [SerializeField] bool Y;
    [SerializeField] bool Z;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        foreach (Transform obj in listOfObjectsToRotate)
        {
            if (X)
                obj.Rotate(Vector3.right, speed * Time.deltaTime);
            if (Y)
                obj.Rotate(Vector3.up, speed * Time.deltaTime);
            if (Z)
                obj.Rotate(Vector3.forward, speed * Time.deltaTime);
        }
    }
}
