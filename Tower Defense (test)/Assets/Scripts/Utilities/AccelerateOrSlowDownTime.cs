using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccelerateOrSlowDownTime : MonoBehaviour {

    private float normalTimeScale;
    [Range(0f,10f)] [SerializeField] private float coef;

    private void Start()
    {
        normalTimeScale = Time.timeScale;
    }


    // Update is called once per frame
    void Update () {

        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;


            if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (GameLogic.instance.isGameOver || GameLogic.instance.isGamePaused || GameLogic.instance.isGameWon)
            {
                RamenerLeTempsALaNormale();
                return;
            }
        }


        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.RightShift))
                RalentirLeTemps();
            else
                AccélérerLeTemps();
        }
        else
            RamenerLeTempsALaNormale();
	}

    private void RamenerLeTempsALaNormale()
    {
        Time.timeScale = normalTimeScale;
    }

    private void RalentirLeTemps()
    {
        Time.timeScale = normalTimeScale;
        Time.timeScale /= coef;
    }

    private void AccélérerLeTemps()
    {
        Time.timeScale = normalTimeScale;
        Time.timeScale *= coef;

    }
}
