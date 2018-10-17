using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInfo : MonoBehaviour {

    public GameObject infos;

    private void OnMouseOver()
    {
        if (infos == null)
            return;

        if (infos.activeSelf == false)
        {
            infos.SetActive(true);
        }
    }
    private void OnMouseExit()
    {
        if (infos == null)
            return;

        if (infos.activeSelf == true)
        {
            infos.SetActive(false);
        }
    }
}
