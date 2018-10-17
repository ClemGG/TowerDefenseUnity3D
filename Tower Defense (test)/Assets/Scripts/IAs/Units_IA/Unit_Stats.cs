using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Stats : MonoBehaviour {
    


    [Header("Vie de l'unité : ")]
    public GameObject hpBar;
    public Color startHealthColor = Color.green;
    public Color endHealthColor = Color.red;

    [Space(10)]

    [HideInInspector] public float currentHealth;
                      public float startHealth = 100f;

    [Space(10)]

    [SerializeField] private Transform[] prefabsToSpawnOnDeath;

    [Space(10)]







    [Header("Attributs de l'unité : ")]

    public float range = 15f;
    public GameObject sphereRange;
    [HideInInspector] public float originalRange;
    public float fireRate = 1f;

    [Space(10)]

    public string[] tagsToDetect;

    [Space(10)]

    public bool CanHitFlyingEnemies;
    public bool CanUseAOE;
    public bool CanSlowDownEnemies;
    [Space(5)]
    public bool isPlayer;
    public bool isTargetForEnemies;
    public bool isWorkshop;
    public bool isGenerator;
    public bool isSpotlight;
    public bool canSeeDuringNight;
    [HideInInspector] public bool isDisabledByEvent; // Passe à true quand l'event Blizzard est actif. Utilisée pour désactiver les scripts des tourelles affectées par l'event.
    






    // Use this for initialization
    void Start()
    {
        currentHealth = startHealth;
        originalRange = range;


        if (sphereRange != null)
        {
            UpdateSphereRadius();
        }

        if (hpBar != null)
        {
            hpBar.SetActive(false);
            Update_HealthBar_UI();
        }
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










    #region Gestion du niveau de santé de l'unité

    private void Update_HealthBar_UI()
    {
        hpBar.transform.GetChild(1).GetComponent<Image>().color = Color.Lerp(endHealthColor, startHealthColor, (currentHealth / startHealth));
        hpBar.transform.GetChild(1).GetComponent<Image>().fillAmount = currentHealth / startHealth;
    }








    public void InflictDmgToUnit(float dmg)
    {

        currentHealth -= dmg;

        if (hpBar != null)
        {
            hpBar.SetActive(true);
            Update_HealthBar_UI();
        }


        CheckDeath();
    }



    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            SpawnPrefabsOnDeath();

            if(!isWorkshop)
                UnitManager.instance.RemoveUnitFromUnitManager(gameObject);
            
            Destroy(gameObject);
        }
    }



    private void SpawnPrefabsOnDeath()
    {
        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {
            Instantiate(prefabsToSpawnOnDeath[i], transform.position, Quaternion.identity);
        }
    } 
    #endregion



}
