
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {


    public List<GameObject> turretsInScene;
    public List<GameObject> buildingsInScene;

    public bool isEventBlizzard;
    public bool isEventMagma;
    
    public static UnitManager instance;


    private void Awake()
    {
        if (instance != null)
        {
            print("More than one UnitManager in scene !");
            return;
        }

        instance = this;
    }











    #region En rapport avec le système Jour-Nuit

    public void DisableTurretsAtNight()
    {
        //On désactive les tourelles la nuit
        foreach (GameObject GOturret in turretsInScene)
        {

            if (!GOturret.GetComponent<Unit_Stats>().canSeeDuringNight || !GOturret.GetComponent<TurretActivation>().linkedNode.isAffectedBySpotlight)
            {

                MonoBehaviour[] scripts = GOturret.GetComponent<TurretActivation>().scriptsToActivate;
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].enabled = false;
                }
            }
        }


        //On active la spotlight la nuit
        foreach (GameObject GObuilding in buildingsInScene)
        {
                GObuilding.GetComponent<TurretActivation>().EnableScriptsAtNightIfSpotlight();
        }
    }

    public void EnableTurretsAtDay()
    {
        //On active les tourelles le jour
        foreach (GameObject GOturret in turretsInScene)
        {

            MonoBehaviour[] scripts = GOturret.GetComponent<TurretActivation>().scriptsToActivate;
            for (int i = 0; i < scripts.Length; i++)
            {
                if (GOturret.GetComponent<TurretActivation>().linkedNode.isPoweredByGenerator)
                {
                    scripts[i].enabled = true;
                }
                else
                {
                    scripts[i].enabled = false;
                }
            }
        }



        //On désactive la spotlight le jour
        foreach (GameObject GObuilding in buildingsInScene)
        {
            GObuilding.GetComponent<TurretActivation>().DisableScriptsAtDayIfSpotlight();
        }
    }
    #endregion











    #region En rapport avec l'event Blizzard
    public void DisableRandomUnitsOnBlizzardEvent(float disableChance, float coefRange)
    {
        foreach (GameObject turret in turretsInScene)
        {
            DisableUnitRandomly(turret, disableChance, coefRange);
        }
        foreach (GameObject building in buildingsInScene)
        {
            DisableUnitRandomly(building, disableChance, coefRange);

        }


    }

    private void DisableUnitRandomly(GameObject unit, float disableChance, float coefRange)
    {

        ReduceRangeOfUnit(unit, coefRange);
        

        if (unit.GetComponent<Unit_Stats>().isGenerator)
        {
            unit.GetComponent<Generator_IA>().RevertNodeState();
        }

        if (unit.GetComponent<Unit_Stats>().isSpotlight)
        {
            unit.GetComponent<Spotlight_IA>().RevertNodeState();
        }



        float alea = Random.Range(0f, 1f);


        if (alea <= disableChance)
        {
            unit.GetComponent<TurretActivation>().ShutDownLinkedNodeIfGenerator();
            unit.GetComponent<Unit_Stats>().isDisabledByEvent = true;
            unit.GetComponent<TurretActivation>().DisableScriptsIfNotPowered();
            unit.GetComponent<TurretActivation>().AssignSnowShader(true);
        }
        else
        {
            if (unit.GetComponent<Unit_Stats>().isGenerator)
            {
                unit.GetComponent<Generator_IA>().ActivateNearbyNodes();
            }

            if (unit.GetComponent<Unit_Stats>().isSpotlight)
            {
                unit.GetComponent<Spotlight_IA>().ActivateNearbyNodes();
            }
        }
    }

    private void ReduceRangeOfUnit(GameObject unit, float coefRange)
    {
        unit.GetComponent<Unit_Stats>().range /= coefRange;
        unit.GetComponent<Unit_Stats>().UpdateSphereRadius();

    }

    public void RevertBlizzardEvent(float coefRange)
    {

        foreach (GameObject turret in turretsInScene)
        {
            turret.GetComponent<Unit_Stats>().range *= coefRange;
            turret.GetComponent<Unit_Stats>().UpdateSphereRadius();
        }
        foreach (GameObject building in buildingsInScene)
        {
            building.GetComponent<Unit_Stats>().range *= coefRange;
            building.GetComponent<Unit_Stats>().UpdateSphereRadius();

            if (building.GetComponent<Unit_Stats>().isGenerator && !building.GetComponent<Unit_Stats>().isDisabledByEvent)
            {
                building.GetComponent<Generator_IA>().ActivateNearbyNodes();
            }

            if (building.GetComponent<Unit_Stats>().isSpotlight && !building.GetComponent<Unit_Stats>().isDisabledByEvent)
            {
                building.GetComponent<Spotlight_IA>().ActivateNearbyNodes();
            }
        }
    }
    #endregion









    #region En rapport avec l'event Magma




    public void BoostUnitsOnMagmaEvent(float coefFireRate, float coefRange)
    {
        foreach (GameObject turret in turretsInScene)
        {
            BoostUnit(turret, coefFireRate, coefRange);
        }
        foreach (GameObject building in buildingsInScene)
        {
            BoostUnit(building, coefFireRate, coefRange);
        }
    }




    private void BoostUnit(GameObject unit, float coefFireRate, float coefRange)
    {

        unit.GetComponent<Unit_Stats>().fireRate /= coefFireRate;
        unit.GetComponent<Unit_Stats>().range *= coefRange;
        unit.GetComponent<Unit_Stats>().UpdateSphereRadius();
    }



    public void RevertMagmaEvent(float coefFireRate, float coefRange)
    {
        foreach (GameObject turret in turretsInScene)
        {
            turret.GetComponent<Unit_Stats>().fireRate *= coefFireRate;
            turret.GetComponent<Unit_Stats>().range /= coefRange;
            turret.GetComponent<Unit_Stats>().UpdateSphereRadius();
        }
        foreach (GameObject building in buildingsInScene)
        {
            building.GetComponent<Unit_Stats>().fireRate *= coefFireRate;
            building.GetComponent<Unit_Stats>().range /= coefRange;
            building.GetComponent<Unit_Stats>().UpdateSphereRadius();
        }
    } 

    #endregion









    public void RegisterTurret(GameObject turret)
    {
        turretsInScene.Add(turret);
        turret.GetComponent<UnitIndex>().indexInUnitManager = turretsInScene.IndexOf(turret);

        if (isEventBlizzard)
            ReduceRangeOfUnit(turret, EventScript.instance.coefRange);
        if (isEventMagma)
            BoostUnit(turret, EventScript.instance.coefFireRate, EventScript.instance.coefRange);
    }



    public void RemoveTurret(GameObject turret)
    {
        //turretsInScene.RemoveAt(turret.GetComponent<UnitIndex>().indexInUnitManager);
        turretsInScene.Remove(turret);

        foreach (GameObject GOturret in turretsInScene)
        {
            if(GOturret != null)
            GOturret.GetComponent<UnitIndex>().indexInUnitManager = turretsInScene.IndexOf(GOturret);
        }
    }






















    public void RegisterBuilding(GameObject building)
    {
        buildingsInScene.Add(building);
        building.GetComponent<UnitIndex>().indexInUnitManager = buildingsInScene.IndexOf(building);


        if (isEventBlizzard)
            ReduceRangeOfUnit(building, EventScript.instance.coefRange);
        if (isEventMagma)
            BoostUnit(building, EventScript.instance.coefFireRate, EventScript.instance.coefRange);
    }






    public void RemoveBuilding(GameObject building)
    {
        //buildingsInScene.RemoveAt(building.GetComponent<UnitIndex>().indexInUnitManager);
        buildingsInScene.Remove(building);
        
        foreach (GameObject GObuilding in buildingsInScene)
        {
            if(GObuilding != null)
            GObuilding.GetComponent<UnitIndex>().indexInUnitManager = buildingsInScene.IndexOf(GObuilding);
        }

        if (building.GetComponent<Unit_Stats>().isGenerator)
        {
            building.GetComponent<Generator_IA>().RevertNodeState();
        }
        else if (building.GetComponent<Unit_Stats>().isSpotlight)
        {
            building.GetComponent<Spotlight_IA>().RevertNodeState();
        }
    }









    public void ReloadBuildingsIfAny()
    {
        foreach (GameObject building in buildingsInScene)
        {
            if (building.GetComponent<Unit_Stats>().isGenerator)
            {
                building.GetComponent<Generator_IA>().RevertNodeState();
                building.GetComponent<Generator_IA>().ActivateNearbyNodes();
            }
            else if (building.GetComponent<Unit_Stats>().isSpotlight)
            {
                building.GetComponent<Spotlight_IA>().RevertNodeState();
                building.GetComponent<Spotlight_IA>().ActivateNearbyNodes();
            }
        }
    }








    public void RemoveUnitFromUnitManager(GameObject go)
    {
        TurretActivation turretActivation = go.GetComponent<TurretActivation>();

        if (turretActivation == null)
            return;

        if (turretActivation.unitType == TurretActivation.UnitType.Turret)
            RemoveTurret(go);
        else
            RemoveBuilding(go);



        if (go.GetComponent<Unit_Stats>().isSpotlight)
        {
            go.GetComponent<Spotlight_IA>().enabled = false;
        }

        if (go.GetComponent<Unit_Stats>().isGenerator)
        {
            go.GetComponent<Generator_IA>().enabled = true;
        }


        if (turretActivation.unitType == TurretActivation.UnitType.Building && !go.GetComponent<Unit_Stats>().isWorkshop)
        {
            ReloadBuildingsIfAny();
        }
    }
}
