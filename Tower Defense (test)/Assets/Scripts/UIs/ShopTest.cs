using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour {

    public int scalePos = 50;
    public bool shopOpened;
    public void SetOut()
    {
        if(!shopOpened)
            transform.Translate(Vector3.left * scalePos);
        else
            transform.Translate(Vector3.right * scalePos);

        shopOpened = !shopOpened;
    }
    
}
