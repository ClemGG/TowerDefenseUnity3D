using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapBetweenCams : MonoBehaviour {

    [SerializeField] private Camera[] cams;
    [SerializeField] private KeyCode toucheSwap;
    [SerializeField] private int currentCam = 0;

    // Use this for initialization
    void Start () {
        //cams = FindObjectsOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(toucheSwap))
        {
            if(currentCam < cams.Length - 1)
            {
                currentCam++;
            }
            else if (currentCam == cams.Length - 1)
            {
                currentCam = 0;
            }

            for (int i = 0; i < cams.Length; i++)
            {
                if(i != currentCam)
                {
                    cams[i].enabled = false;
                }
                else
                {
                    cams[i].enabled = true;

                }
            }
        }
	}
}
