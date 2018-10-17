using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour {

    private ColorBlock startColorBlock;
    public ColorBlock readyColorBlock;
   

	// Use this for initialization
	void Start () {
        startColorBlock = GetComponent<Button>().colors;
	}
	
	// Update is called once per frame
	public void Start_ColorBlocks ()
    {
         GetComponent<Button>().colors = startColorBlock;
    }
    public void Ready_ColorBlocks ()
    {
         GetComponent<Button>().colors = readyColorBlock;
    }
}
