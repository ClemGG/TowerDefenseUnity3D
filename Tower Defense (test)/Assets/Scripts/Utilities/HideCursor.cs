using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour {

    [SerializeField] private bool hideCursor;


	// Use this for initialization
	void Start () {
        if (hideCursor)
            Cursor.visible = false;
	}
}
