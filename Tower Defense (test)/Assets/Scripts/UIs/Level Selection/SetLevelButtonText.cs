using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetLevelButtonText : MonoBehaviour {

    public bool resetPlayerPrefs;


	void Start () {

        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (i+1).ToString();

            if (i+1 > levelReached)
                transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
	}

}
