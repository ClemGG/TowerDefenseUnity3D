using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

    [SerializeField] private float delay;
    [SerializeField] private bool isUsingAudio;

    // Use this for initialization
    void Start () {
        if (isUsingAudio)
            Destroy(gameObject, GetComponent<AudioSource>().clip.length);
        else
            Destroy(gameObject, delay);
    }

}
