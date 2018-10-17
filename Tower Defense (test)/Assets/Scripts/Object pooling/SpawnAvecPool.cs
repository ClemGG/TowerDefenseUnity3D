using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAvecPool : MonoBehaviour {

    
    ObjectPooler objectPooler;
    [SerializeField] private Transform listOfPaths;

    [Space]
    [Space]
    [Space]

    [SerializeField] private string tagToSpawn;
    private float timer = 0;
    [SerializeField] private float instantiateDelay;
    [SerializeField] private int nbEnemisMax = 10;

    private int nbInstancesInScene;
    private Pool currentPoolUsed;

    [Space]
    [Space]
    [Space]
    
    [SerializeField] private Transform[] prefabsToInstantiateWithEnemies;




    void OnEnable()
    {
        objectPooler = ObjectPooler.instance;
        
            
        foreach (Pool pool in objectPooler.Pools)
        {
            if (pool.tag == tagToSpawn)
                currentPoolUsed = pool;
        }
        
        timer = 0;
        Spawn();

    }



    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer > instantiateDelay)
        {
            Spawn();
        }
        
    }
    

    public void Spawn()
    {
        if (nbEnemisMax <= 0 || nbInstancesInScene >= currentPoolUsed.size)
        {
            enabled = false;
            return;
        }

        Waypoints path = SelectRandomPath();
        GameObject go = objectPooler.SpawnFromPool(tagToSpawn, path.waypoints[0].position, Quaternion.identity);
        go.GetComponent<EnemyIA>().pathToFollow = path;


        SpawnPrefabsOnInstantiate(path.waypoints[0].position);
        timer = 0;
        nbInstancesInScene++;
        nbEnemisMax--;

    }





    private Waypoints SelectRandomPath()
    {
        return listOfPaths.GetChild(Random.Range(0, listOfPaths.childCount)).GetComponent<Waypoints>();
    }








    private void SpawnPrefabsOnInstantiate(Vector3 spawnPos)
    {
        for (int i = 0; i < prefabsToInstantiateWithEnemies.Length; i++)
        {
            Instantiate(prefabsToInstantiateWithEnemies[i], spawnPos, Quaternion.identity);
        }
    }


}
