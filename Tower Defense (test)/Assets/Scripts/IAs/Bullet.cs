using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    private Vector3 birthPoint;
    [HideInInspector] public Transform target;
    public float speed;
    public float range;
    public float dmg;
    
    public Transform[] prefabsToSpawnOnDeath;




    public void Seek(Transform _target, float _dmg)
    {
        if (target == null)
        {
            target = _target;
        }
        
        dmg = _dmg;
    }




    // Use this for initialization
    void Start () {
        birthPoint = transform.position;
	}

	
	// Update is called once per frame
	void Update () {

        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveBullet();
        DestroyAfterRange();
    }






    private void DestroyAfterRange()
    {
        float distance = Vector3.Distance(transform.position, birthPoint);

        if (distance >= range)
        {
             Destroy(gameObject);
        }


    }

    
    private void MoveBullet()
    {
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }





    private void HitTarget()
    {

        if (target.gameObject.activeSelf == false)
            return;
        
        if (target.GetComponent<EnemyStats>())
        {
            target.GetComponent<EnemyStats>().InflictDmg(dmg);
        }
        if (target.GetComponent<Unit_Stats>())
        {
            if (target.GetComponent<Unit_Stats>().isPlayer)
            {
                PlayerStats.instance.InflictDmg(dmg);
            }
            else
            {
                target.GetComponent<Unit_Stats>().InflictDmgToUnit(dmg);
            }
        }

        SpawnPrefabsOnDeath();
        Destroy(gameObject);

    }




    private void SpawnPrefabsOnDeath()
    {
        for (int i = 0; i < prefabsToSpawnOnDeath.Length; i++)
        {
            Instantiate(prefabsToSpawnOnDeath[i], transform.position, Quaternion.identity);
        }
    }

    
}
    

