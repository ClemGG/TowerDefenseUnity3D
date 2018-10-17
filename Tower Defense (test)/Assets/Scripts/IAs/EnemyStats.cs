using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour {


    public GameObject hpBar;
    public Color startHealthColor = Color.green;
    public Color endHealthColor = Color.red;

    [Space(10)]


    public string[] tagsToDetect;
    [Space(10)]

    [Space(10)]


    [Header("Attributs de l'unité : ")]


    [HideInInspector] public float currentHealth;
    public float startHealth = 100f;

    public int moneyDropped = 1;

    [Header("Attributs de l'unité : ")]

    public float dmgToInflictToPlayer = 10f;
    public bool dealDmgOnDeath;
    public float range = 15f;
    public GameObject sphereRange;
    [HideInInspector] public float originalRange;
    public float fireRate = 1f;



    [Space(10)]

    [SerializeField] private Transform[] prefabsToSpawnOnDeath;






    // Use this for initialization
    void OnEnable () {

        originalRange = range;

        if(sphereRange != null)
        {
            UpdateSphereRadius();
            sphereRange.SetActive(false);
        }
        


        currentHealth = startHealth;
        Update_HealthBar_UI();
        hpBar.SetActive(false);
    }












    private void Update_HealthBar_UI()
    {
        hpBar.transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(endHealthColor, startHealthColor, (currentHealth / startHealth));
        hpBar.transform.GetChild(1).GetComponent<Image>().fillAmount = currentHealth / startHealth;
    }


    public void UpdateSphereRadius()
    {
        sphereRange.transform.localScale = new Vector3(range * 2f, range * 2f, range * 2f);
    }


    public void PlaySound(bool play)
    {
        if (!GetComponent<AudioSource>())
            return;


        if (GetComponent<AudioSource>().clip == null)
            return;

        if (play && !GetComponent<AudioSource>().isPlaying)
        {
            //print("Play");
            GetComponent<AudioSource>().Play();
        }
        if (!play)
        {
            //print("Stop");
            GetComponent<AudioSource>().Stop();
        }
    }





    public void InflictDmg(float dmg)
    {

        hpBar.SetActive(true);


        currentHealth -= dmg;
        Update_HealthBar_UI();
        CheckDeath();
    }



    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            PlayerStats.instance.IncreaseMoneyBy(moneyDropped);
            SpawnPrefabsOnDeath();
            gameObject.SetActive(false);
        }
    }



    public void SpawnPrefabsOnDeath()
    {
        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {
            Instantiate(prefabsToSpawnOnDeath[i], transform.position, Quaternion.identity);
        }
    }


}

