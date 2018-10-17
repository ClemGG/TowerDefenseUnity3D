
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour {

    public EnemyStats enemyStats;


    [Space(10)]


    [HideInInspector] public float speed;
    [HideInInspector] public bool isSlowedDown;

    public Transform partToRotate;
    public float startSpeed = 20f;
    public float rotationSpeed = 20f;
    public float waypointLimitRadius = .2f;

    [Space(10)]

    
    private Transform targetedUnit;
    public Transform[] canonsToRotate;
    [Space(10)]
    public Transform bulletPrefab;
    public Transform[] spawnPoints;


    [Space(10)]


    private Transform targetedWaypoint;
    [HideInInspector] public Waypoints pathToFollow;
    private int waypointIndex = 0;


    private float timer = 0f;







    private void OnEnable()
    {
       enemyStats = GetComponent<EnemyStats>();
    }





    private void OnDisable()
    {
        waypointIndex = 0;
    }


    private void Start()
    {
        InvokeRepeating("Update_Target", 0f, .5f);
        speed = startSpeed;
        targetedWaypoint = pathToFollow.waypoints[waypointIndex];
    }

    private void Update()
    {

        MoveEnemy();
        Update_Rotation();
        Shoot_Player_If_Within_Range();
    }














    private void Shoot_Player_If_Within_Range()
    {
        Shoot_If_Target_Is_Not_Null();
        Update_Canon_Rotation();
    }



    private void Update_Target()
    {

        if (canonsToRotate == null)
        {
            return;
        }


        float shortestDistance = Mathf.Infinity;
        GameObject nearestUnit = null;




        Unit_Stats[] units = FindObjectsOfType<Unit_Stats>();


        if(units == null)
        {
            return;
        }




        foreach (Unit_Stats unit in units)
        {
                


            if (unit.isTargetForEnemies)
            {

                float distanceToUnit = Vector3.Distance(transform.position, unit.transform.position);

                if (distanceToUnit < shortestDistance)
                {
                    shortestDistance = distanceToUnit;
                    nearestUnit = unit.gameObject;
                }
            }
        }
        




            if (targetedUnit == null)
            {
                if (nearestUnit != null && shortestDistance <= enemyStats.range)
                {
                    targetedUnit = nearestUnit.transform;
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, targetedUnit.position) > enemyStats.range || !targetedUnit.gameObject.activeSelf)
                {
                    targetedUnit = null;
                }
            }
    }















    private void Shoot_If_Target_Is_Not_Null()
    {




        timer -= Time.deltaTime;

        if (timer <= 0f && targetedUnit != null)
        {
            Shoot();
            timer = enemyStats.fireRate;
        }


        if(targetedUnit == null)
        {
            enemyStats.PlaySound(false);
        }
    }

    private void Shoot()
    {
        enemyStats.PlaySound(true);

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform bulletGO = Instantiate(bulletPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Seek(targetedUnit, enemyStats.dmgToInflictToPlayer);
            }
        }
    }










    private void Update_Canon_Rotation()
    {
        if (targetedUnit != null)
        {
            foreach (Transform canon in canonsToRotate)
            {
                canon.LookAt(targetedUnit);
            }
        }
    }

    private void Update_Rotation()
    {
        if (targetedWaypoint != null)
        {
            Vector3 direction = targetedWaypoint.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(partToRotate.localRotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            partToRotate.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
    }












    private void MoveEnemy()
    {
        Vector3 direction = targetedWaypoint.position - transform.position;

        transform.position += direction.normalized * speed * Time.deltaTime;
        //transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetedWaypoint.position) <= waypointLimitRadius)
        {
            GetNextPoint();
        }
    }

    private void GetNextPoint()
    {
        if(waypointIndex >= pathToFollow.waypoints.Length - 1)
        {
            if(enemyStats.dealDmgOnDeath)
                PlayerStats.instance.InflictDmg(GetComponent<EnemyStats>().dmgToInflictToPlayer);

            GetComponent<EnemyStats>().SpawnPrefabsOnDeath();
            gameObject.SetActive(false);
            //Destroy(gameObject);
            return;
        }

        waypointIndex++;
        targetedWaypoint = pathToFollow.waypoints[waypointIndex];

    }
}
