using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Msg : MonoBehaviour {

    public GameObject errorPanel;
    public Text errorText;
    public bool useLogConsole;

    public enum ErrorType { Error, Warning, Info }
    public ErrorType errorType;


    public static Msg instance;




    private void Awake()
    {
        if (instance != null)
        {
            instance.Print(Msg.ErrorType.Error, 25, "More than one Msg in scene !", true, false);
            return;
        }

        instance = this;

    }

    public void Print(ErrorType errorType, int newFontSize, string msg, bool writeMsgInLogConsole, bool usePopup)
    {
        useLogConsole = writeMsgInLogConsole;
        errorText.fontSize = newFontSize;


        if (errorPanel != null && usePopup)
        {
            if (errorType == ErrorType.Error)
                errorText.text = "<color=#aa0000ff><b>Erreur : </b></color>" + msg;
            if (errorType == ErrorType.Warning)
                errorText.text = "<color=yellow><b>Attention : </b></color>" + msg;
            if (errorType == ErrorType.Info)
                errorText.text = "<color=white><b>Info : </b></color>" + msg;

            Disable_Popup();
            Enable_Popup();
            StartCoroutine(StopPopUp(5f));
        }

        if (useLogConsole)
        {
            if (errorType == ErrorType.Error)
                Debug.Log("<color=#aa0000ff><b>Error : </b></color>" + msg);
            if (errorType == ErrorType.Warning)
                Debug.Log("<color=yellow><b>Warning : </b></color>" + msg);
            if (errorType == ErrorType.Info)
                Debug.Log("<color=white><b>Info : </b></color>" + msg);
        }
    }


    private IEnumerator StopPopUp(float delay)
    {
        errorPanel.GetComponent<Animator>().Play("popupOn");
        yield return new WaitForSeconds(delay);
        errorPanel.GetComponent<Animator>().Play("popupOff");

    }



    private void Enable_Popup()
    {
        errorPanel.SetActive(true);
    }

    private void Disable_Popup()
    {
        errorPanel.SetActive(false);

    }
}
