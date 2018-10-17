using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight_IA : MonoBehaviour {

    public List<Node> nodesAffectedBySpotLight;
    public Light[] spotLights;

    [Space(10)]

    public Unit_Stats unitStats;

    [Space(10)]

    private Transform target;
    public Transform listOfPathsToLookAt;
    public Transform partToRotate;
    public float rotationSpeed = 30f;

    // Use this for initialization
    void Start()
    {
        ToggleLights(true);
        listOfPathsToLookAt = GameObject.FindGameObjectWithTag("Paths").transform;

        nodesAffectedBySpotLight = new List<Node>();
        unitStats = GetComponent<Unit_Stats>();
        ActivateNearbyNodes();

        SeekNewTarget();
    }






    private void UpdateRotation()
    {


        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            partToRotate.localRotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    public void SeekNewTarget()
    {
        float distanceToSpotlight = Mathf.Infinity;

        foreach (Transform path in listOfPathsToLookAt)
        {
            foreach (Transform waypoint in path)
            {
                float temp = Vector3.Distance(transform.position, waypoint.position);
                if (temp < distanceToSpotlight)
                {
                    distanceToSpotlight = temp;
                    target = waypoint;
                }
            }
        }

        UpdateRotation();
    }

















    public void ActivateNearbyNodes()
    {
        Collider[] nodesWithinRange = Physics.OverlapSphere(transform.position, unitStats.range);

        foreach (Collider node in nodesWithinRange)
        {
            if (node.GetComponent<Node>())
                nodesAffectedBySpotLight.Add(node.GetComponent<Node>());
        }

        for (int i = 0; i < nodesAffectedBySpotLight.Count; i++)
        {
            nodesAffectedBySpotLight[i].isAffectedBySpotlight = true;


            if (nodesAffectedBySpotLight[i].turretOnThisNode != null)
            {
                nodesAffectedBySpotLight[i].turretOnThisNode.GetComponent<TurretActivation>().EnableScriptsAtNightIfTurretFromSpotlight();
            }
        }

        if(listOfPathsToLookAt != null)
            SeekNewTarget();
    }









    public void RevertNodeState()
    {

        for (int i = 0; i < nodesAffectedBySpotLight.Count; i++)
        {
            nodesAffectedBySpotLight[i].isAffectedBySpotlight = false;

            if (nodesAffectedBySpotLight[i].turretOnThisNode != null)
            {
                nodesAffectedBySpotLight[i].turretOnThisNode.GetComponent<TurretActivation>().DisableScriptsAtNightIfTurret();
            }
        }

        nodesAffectedBySpotLight.Clear();

       
    }
    





    public void ToggleLights(bool on)
    {
        for (int i = 0; i < spotLights.Length; i++)
        {
            spotLights[i].gameObject.SetActive(on);
        }
    }
}
