using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public bool infiniteHealthAndMoney;
    
    [HideInInspector] public int currentMoney;
    public int startMoney = 10;
    [HideInInspector] public float currentHealth;
    public float startHealth = 100;


    public Text moneyText;
    public Slider healthSlider;

    [Space]
    [Space]
    [Space]

    public Transform endNode;
    private Vector3 positionOffset = new Vector3(0f, .5f, 0f);
    public Transform[] prefabsToSpawnOnHit;

    [Space]
    [Space]
    [Space]
    
    public Transform[] prefabsToSpawnOnDeath;

    public static PlayerStats instance;


    public event Action OnMoneyChanged;


    private void Awake()
    {
        if (instance != null)
        {
            print("More than one PlayerStats in scene !");
            return;
        }

        instance = this;

    }

    // Use this for initialization
    void Start () {


        Setup_Stats();
        CheckDeath();

    }

    public void Setup_Stats()
    {

        if (infiniteHealthAndMoney)
        {
            currentMoney = 10000;
            currentHealth = 10000;
        }
        else
        {
            currentMoney = startMoney;
            currentHealth = startHealth;
        }
        moneyText.text = currentMoney.ToString();
        healthSlider.value = 1f;
    }

    public Vector3 GetEndNodePosition()
    {
        return endNode.position + positionOffset;
    }









    public void InflictDmg (float dmg)
    {
        currentHealth -= dmg;


        CheckDeath();

        healthSlider.value = currentHealth / startHealth;

        SpawnPrefabsOnHit();
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        SpawnPrefabsOnDeath();

        if(!GameLogic.instance.isGameOver)
            GameLogic.instance.StartGameOver();
    }









    private void SpawnPrefabsOnDeath()
    {
        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {
            Instantiate(prefabsToSpawnOnDeath[i], GetEndNodePosition(), Quaternion.identity);
        }
    }

    private void SpawnPrefabsOnHit()
    {
        for (int i = 0; i < prefabsToSpawnOnHit.Length; i++)
        {
            Instantiate(prefabsToSpawnOnHit[i], GetEndNodePosition(), Quaternion.identity);
        }
    }








    public void ReduceMoneyBy(int cost)
    {
        if (currentMoney >= cost)
        {
            currentMoney -= cost;
            //Debug.Log("Turret built ! currentMoney left : " + currentMoney + ".");
        }

        if (currentMoney < 0)
        {
            currentMoney = 0;
        }

        Shop.instance.Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney();
        moneyText.text = currentMoney.ToString();
    }

    public void IncreaseMoneyBy(int amount)
    {
        currentMoney += amount;
        Shop.instance.Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney();





        //Debug.Log("You gained " + amount + " Energy Points. CurrentMoney :" + currentMoney + ".");
        moneyText.text = currentMoney.ToString();
    }
}
