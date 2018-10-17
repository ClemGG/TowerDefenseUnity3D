using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEExplosion : MonoBehaviour {

    public float range = 7f;
    public int dmg;
    public string[] tagsToDetect;

    // Use this for initialization
    void Start ()
    {
      Explode();
    }

    private void Explode()
    {


        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach (Collider col in colliders)
        {
            foreach (string tag in tagsToDetect)
            {

                if (col.tag == tag)
                {
                    col.GetComponent<EnemyStats>().InflictDmg(dmg);
                }
            }
        }
        Destroy(gameObject);

    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
