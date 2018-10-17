using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(Addition(1,2,3));
	}

    int Addition(int a, int b)
    {
        return a + b;
    }
    int Addition(int a, int b, int c)
    {
        return a + b + c;
    }
    string Addition(string a, string b)
    {
        return a + b;
    }
}
