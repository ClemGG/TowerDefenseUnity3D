using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {
    
    public Transform spawners;
    public Text countdownText;
    //public Text WaveIDText;
    public Button buttonSpawnnextWave;

    public float delayBetweenWaves = 60f;
    private int currentWaveIndex = 0;
    public Text curentwaveTexte;
    public float moneyCountdownCoef = 1f;
    private bool isLastWave = false;

    [Space]
    [Space]
    [Space]

    public float countdown = 30f;


    public Color startColor;
    public Color endColor;





    public static WaveSpawner instance;

    private void Awake()
    {
        if (instance != null)
        {
            print("More than one WaveSpawner in scene !");
            return;
        }

        instance = this;

    }






    public int NumberOfRoundsSurvived
    {
        get
        { return currentWaveIndex-1; }
    }





    private void Start()
    {
        //WaveIDText.text = "0";
        InvokeRepeating("CheckWinCondition", 0f, 1f);
    }


    // Update is called once per frame
    void Update () {

        

        if (countdown <= 0)
        {
            SpawnNextWave();
        }

        countdown -= Time.deltaTime;
        Update_Countdown_UI();


        curentwaveTexte.text = currentWaveIndex.ToString();
    }












    private void Update_Countdown_UI()
    {
        countdownText.color = Color.Lerp(endColor, startColor, countdown/delayBetweenWaves);

        if(!isLastWave)
            countdownText.text = Mathf.Ceil(countdown).ToString();
        else
            countdownText.text = null;
    }













    public void SpawnNextWave()
    {

        //GivePlayerMoneyDependingOnCountdownLeft();


        for (int i = 0; i < spawners.childCount; i++)
        {
            if(i == currentWaveIndex)
            {
                spawners.GetChild(i).gameObject.SetActive(true);
            }

            //Pas besoin de désactiver les autres spawners via un "else"; ils se désactivent d'eux-mêmes quand ils n'ont plus d'ennemis à spawner. Comme ça, ça permet de lancer plusieurs vagues à la fois. 
            
        }

        if (currentWaveIndex < spawners.childCount - 1)
        {
            currentWaveIndex++;
            //WaveIDText.text = (int.Parse(WaveIDText.text) + 1).ToString();
            buttonSpawnnextWave.interactable = true;
        }
        else
        {
            buttonSpawnnextWave.interactable = false;
            isLastWave = true;
            //WaveIDText.text = (int.Parse(WaveIDText.text)+1).ToString() + " (Finale)";

            //Debug.Log("Plus de vagues d'ennemis.");
        }

        countdown = delayBetweenWaves;


        EventScript.instance.DisableCurrentEventAfterNumberOfWavesPassed(true);

    }











    private void GivePlayerMoneyDependingOnCountdownLeft()
    {
        PlayerStats.instance.IncreaseMoneyBy(Mathf.RoundToInt(countdown * moneyCountdownCoef) * 10);
    }


    public void CheckWinCondition()
    {
        if (isLastWave && FindObjectOfType<EnemyIA>() == null && !GameLogic.instance.isGameWon && PlayerStats.instance.currentHealth > 0f)
        {
            GameLogic.instance.Win();
        }
    }
}
