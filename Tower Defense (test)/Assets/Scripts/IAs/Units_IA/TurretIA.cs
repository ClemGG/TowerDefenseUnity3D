using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIA : MonoBehaviour {

    private Tirer tirerScript;
    private TurretActivation activation;
    public Unit_Stats unitStats;
    [HideInInspector] public Transform target;
    public Transform partToRotate;

    
    public float rotationSpeed = 30f;







    // Use this for initialization
    void Start () {
        InvokeRepeating("Update_Target", 0f, .07f);
        tirerScript = GetComponent<Tirer>();
        unitStats = GetComponent<Unit_Stats>();
        activation = GetComponent<TurretActivation>();
	}








    public void Update_Target()
    {
        

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (string tag in unitStats.tagsToDetect)
        {


            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);


            foreach (GameObject enemy in enemies)
            {
                
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }



            if (target == null)
            {
                if (nearestEnemy != null && shortestDistance <= unitStats.range)
                {
                    target = nearestEnemy.transform;
                    tirerScript.SlowDown_Enemy_If_Unit_Can(target);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, target.position) > unitStats.range || !target.gameObject.activeSelf)
                {
                    tirerScript.Revert_SlowDown_If_Unit_Can(target);
                    target = null;
                }
            }
        }
    }

    

    // Update is called once per frame
    void Update ()
    {

        Update_Rotation();

    }

    private void Update_Rotation()
    {

        if (activation.isNotBuiltYet)
            return;

        if (target != null)
        {
            //Vector3 direction = target.position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //Vector3 rotation = Quaternion.Lerp(partToRotate.localRotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            //partToRotate.localRotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

            partToRotate.LookAt(target);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, unitStats.range);
    //}




    private void OnDestroy()
    {
        if (target == null)
            return;

        if (unitStats.CanSlowDownEnemies)
        {
            if (!target.GetComponent<EnemyIA>().isSlowedDown && target.GetComponent<EnemyIA>().speed == target.GetComponent<EnemyIA>().startSpeed / tirerScript.slowFactor)
            {
                tirerScript.Revert_SlowDown_If_Unit_Can(target);
            }
        }
    }
}