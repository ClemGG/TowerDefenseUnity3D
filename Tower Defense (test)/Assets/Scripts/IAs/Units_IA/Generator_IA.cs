using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator_IA : MonoBehaviour {
    
    public List<Node> nodesAffectedByGenerator;
    public Unit_Stats unitStats;


    // Use this for initialization
    void Start () {
        nodesAffectedByGenerator = new List<Node>();
        unitStats = GetComponent<Unit_Stats>();
        ActivateNearbyNodes();
	}






    public void ActivateNearbyNodes()
    {
        Collider[] nodesWithinRange = Physics.OverlapSphere(transform.position, unitStats.range);

        foreach (Collider node in nodesWithinRange)
        {
            if(node.GetComponent<Node>())
                nodesAffectedByGenerator.Add(node.GetComponent<Node>());
        }

        for (int i = 0; i < nodesAffectedByGenerator.Count; i++)
        {
            nodesAffectedByGenerator[i].isPoweredByGenerator = true;

            if(nodesAffectedByGenerator[i].turretOnThisNode != null)
            {
                if (!nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Unit_Stats>().isSpotlight)
                {
                    nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().EnableScriptsIfPowered();
                }
                else
                {
                    if (!nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Unit_Stats>().isDisabledByEvent)
                    {
                        nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().EnableScriptsAtNightIfSpotlight();
                        nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Spotlight_IA>().ActivateNearbyNodes();
                    }
                }


                nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().DisableScriptsAtNightIfTurret();

                if (nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Unit_Stats>().isDisabledByEvent)
                {
                    nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().DisableScriptsDuringBlizzardEventIfGenerator();
                }
            }

        }
    }








    public void RevertNodeState()
    {

        for (int i = 0; i < nodesAffectedByGenerator.Count; i++)
        {
            nodesAffectedByGenerator[i].isPoweredByGenerator = false;


            if (nodesAffectedByGenerator[i].turretOnThisNode != null)
            {
                //Si c'est un générateur, on ne désactive pas ses scripts; sinon, il ne pourrait plus se mettre à jour!
                if (!nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Unit_Stats>().isGenerator)
                    nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().DisableScriptsIfNotPowered();

                //Si c'est une spotlight, on s'assure de désactiver ses scripts et lights après avoir ramené ses nodes à la normale
                if (nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<Unit_Stats>().isSpotlight)
                {
                    nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().DisableSpotlightFromGenerator();
                }


                nodesAffectedByGenerator[i].turretOnThisNode.GetComponent<TurretActivation>().PowerLinkedNodeIfGenerator();
            }
        }

        nodesAffectedByGenerator.Clear();
    }




    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, unitStats.range);
    //}
}
