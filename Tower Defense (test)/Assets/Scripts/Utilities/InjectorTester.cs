using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectorTester : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        //Msg.print(Msg.ErrorType.Error, "lol");
        //Msg.print(Msg.ErrorType.Warning, "lol");
        //Msg.print(Msg.ErrorType.Info, "lol");

        //StartCoroutine(test());
	}

    private IEnumerator test()
    {
        float timer = 0f;

        while (timer <= 60f)
        {
            timer += Time.deltaTime;
            print(timer);
            yield return null;
        }
    }
}
