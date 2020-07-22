using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirer : MonoBehaviour{



    [Header("Projectile Properties : ")]
    [Range(1f,10f)] public float slowFactor; // Entre 1 et 10 ; le mettre à 1 laissera l'ennemi à sa vitesse initiale
    public Transform bulletPrefab;
    public Transform[] spawnPoints;
    [HideInInspector] TurretIA ia;
    [HideInInspector] TurretActivation activation;
    [HideInInspector] public Unit_Stats unitStats;

    private float timer = 0f;

    


    // Use this for initialization
    void Start () {
        ia = GetComponent<TurretIA>();
        unitStats = GetComponent<Unit_Stats>();
        activation = GetComponent<TurretActivation>();
        
	}
	
	// Update is called once per frame
	void Update () {

        

        timer -= Time.deltaTime;

        if(timer <= 0f && ia.target != null)
        {
            Shoot();
            timer = unitStats.fireRate;
        }


        if (ia.target == null)
            unitStats.PlaySound(false);

    }






    //Appelée par TurretIA
    public void SlowDown_Enemy_If_Unit_Can(Transform target)
    {
        if (unitStats.CanSlowDownEnemies)
        {
            target.GetComponent<EnemyIA>().isSlowedDown = true;
            target.GetComponent<EnemyIA>().speed = target.GetComponent<EnemyIA>().startSpeed / slowFactor;
        }
            
    }

    //Appelée par TurretIA
    public void Revert_SlowDown_If_Unit_Can(Transform target)
    {
        if (unitStats.CanSlowDownEnemies)
        {
            target.GetComponent<EnemyIA>().speed = target.GetComponent<EnemyIA>().startSpeed;
            target.GetComponent<EnemyIA>().isSlowedDown = false;
        }
    }









    private void Shoot()
    {
        if (activation.isNotBuiltYet)
            return;

        unitStats.PlaySound(true);


        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform bulletGO = Instantiate(bulletPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Seek(ia.target, bullet.dmg);
            }
        }
    }
}
