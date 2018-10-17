using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilder : MonoBehaviour {


    private TurretBlueprint unitToBuild;
    private Node selectedNode;
    
    
    public NodeUi nodeUI;
    public string[] prefabsInstantiableWhenNotPowered;




    public static TurretBuilder instance;
    public static Atelier_IA atelierInstance;

    [Space(10)]

    public int currentWorkshopLevel;


    private void Awake()
    {
        if (instance != null)
        {
            print("More than one TurretBuilder in scene !");
            return;
        }

        instance = this;
        currentWorkshopLevel = 0;

    }









    public bool HasAnUnitToBuild
    {
        get { return unitToBuild != null; }
    }
    public bool HasEnoughMoneyToBuildNextUnit
    {
        get { return PlayerStats.instance.currentMoney >= unitToBuild.cost; }
    }






    public void Set_Unit_To_Build(TurretBlueprint purchasedUnit)
    {
        unitToBuild = purchasedUnit;
    }
    public TurretBlueprint Get_Unit_To_Build()
    {
        return unitToBuild;
    }


    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.HideUI();
    }









    public bool IsTurretEligibleForBuildOnNode(Node node, TurretBlueprint turretToBuild)
    {

        if (node.unitToPlaceOnAwake.turret != null)
            return true;


        bool canTurretBeBuilt = false;
        

            if (!node.isPoweredByGenerator)
            {
                for (int i = 0; i < prefabsInstantiableWhenNotPowered.Length; i++)
                {
                    if (string.Compare(turretToBuild.turret.name.Trim(), prefabsInstantiableWhenNotPowered[i].Trim()) == 0)
                        canTurretBeBuilt = true;
                }

            }
            else
            {
                canTurretBeBuilt = true;
            }
        return canTurretBeBuilt;
            
    }



    public void BuildUnitOnNode(Node node, TurretBlueprint blueprintToUpgrade)
    {
        if (!IsTurretEligibleForBuildOnNode(node, unitToBuild))
        {
            unitToBuild.associatedButton.gameObject.transform.GetChild(4).gameObject.SetActive(false);
            unitToBuild.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
            return;
        }






        if (unitToBuild.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            if (atelierInstance != null)
            {
                Msg.instance.Print(Msg.ErrorType.Error, 25, "More than one Atelier in scene !", true, false);
                return;
            }

            atelierInstance = unitToBuild.turret.GetComponent<Atelier_IA>();
        }
        




        if (!unitToBuild.turret.GetComponent<Unit_Stats>().isWorkshop)
        {


            if (PlayerStats.instance.currentMoney < unitToBuild.cost)
            {
                Msg.instance.Print(Msg.ErrorType.Error, 25, "Pas assez d'argent pour acheter l'upgrade  " + unitToBuild.turret.name, true, true);
                return;
            }

        }

        Shop.instance.Set_LockState_For_All_Buttons(currentWorkshopLevel);
        PlayerStats.instance.ReduceMoneyBy(unitToBuild.cost);

        node.turretOnThisNode = Instantiate(unitToBuild.turret, node.GetBuildPosition(), unitToBuild.turret.transform.localRotation);
            SpawnUnitPrefabsOnNode(node);


        if (!unitToBuild.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            AddUnitsToUnitManager(node, blueprintToUpgrade);
        }
        
        


        node.turretOnThisNode.GetComponent<TurretActivation>().StartCoroutine("WaitBeforeCreatingNewTurret", blueprintToUpgrade.buildDelay);
        node.turretOnThisNode.GetComponent<TurretActivation>().linkedNode = node;




        if (!unitToBuild.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            unitToBuild.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();


            //Shop.instance.Enable_Unit_Buttons();
        }



        Shop.instance.ToggleNodesVisibility(false);
        Shop.instance.DestroyPreview();


        //Pour ne poser qu'une tourelle à la fois
        Set_Unit_To_Build(null);


    }









    public void UpgradeUnitOnNode(Node node, ref int currentUpgradeVersion, TurretBlueprint blueprintToUpgrade)
    {



        if (blueprintToUpgrade.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            currentWorkshopLevel++;
            Shop.instance.Set_LockState_For_All_Buttons(currentWorkshopLevel);
            atelierInstance = blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.GetComponent<Atelier_IA>();
        }


        PlayerStats.instance.ReduceMoneyBy(blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeCost);

        if (!blueprintToUpgrade.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            RemoveUnitsFromUnitManagerAndReloadBuildings(node, blueprintToUpgrade);
        }


        node.turretOnThisNode = Instantiate(blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret, node.GetBuildPosition(), blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.transform.localRotation);

        if (!blueprintToUpgrade.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            AddUnitsToUnitManager(node, blueprintToUpgrade);
        }

        node.turretOnThisNode.GetComponent<TurretActivation>().linkedNode = node;
        node.turretOnThisNode.GetComponent<TurretActivation>().isAnUpgrade = true;


        

        SpawnUpgradePrefabsOnNode(node);
        currentUpgradeVersion++;

        if (currentUpgradeVersion == blueprintToUpgrade.listOfUpgrades.Length && currentUpgradeVersion > 0)
        {
            nodeUI.DisableUpgradeButton(40, "Aucune amélioration restante.");
        }

        //print("Node :" + node.name + " Node version :" + currentUpgradeVersion + " Worksshop : " + currentWorkshopLevel);


        if (Shop.instance.preview != null)
            Shop.instance.preview.SetActive(false);

    }















    public void SellUnitOnNode(Node node, ref int currentUpgradeVersion, TurretBlueprint blueprintToUpgrade)
    {


        if (blueprintToUpgrade.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            Msg.instance.Print(Msg.ErrorType.Error, 25, "Vous ne pouvez pas vendre l'atelier.", true, true);
            return;
        }



        bool unitToSellIsAnUpgrade = false;


        for (int i = 0; i < blueprintToUpgrade.listOfUpgrades.Length; i++)
        {
            if (node.turretOnThisNode.name.Replace("(Clone)", string.Empty) == blueprintToUpgrade.listOfUpgrades[i].upgradeTurret.name)
            {
                unitToSellIsAnUpgrade = true;
            }
            break;
        }

        if (unitToSellIsAnUpgrade)
        {
            PlayerStats.instance.IncreaseMoneyBy(blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion - 1].upgradeSellIncome);
            SpawnUpgradePrefabsOnNode(node);
        }
        else
        {
            PlayerStats.instance.IncreaseMoneyBy(blueprintToUpgrade.sellIncome);

            TurretBlueprint tb = unitToBuild;
            
            Set_Unit_To_Build(blueprintToUpgrade);
            SpawnUnitPrefabsOnNode(node);
            Set_Unit_To_Build(tb);
        }




        RemoveUnitsFromUnitManagerAndReloadBuildings(node, blueprintToUpgrade);
        

        blueprintToUpgrade = null;
        currentUpgradeVersion = 0;

        nodeUI.EnableUpgradeButton();

        if (Shop.instance.preview != null)
            Shop.instance.preview.SetActive(false);

    }










    private void AddUnitsToUnitManager(Node node, TurretBlueprint blueprintToUpgrade)
    {
        

        if (blueprintToUpgrade.unitType == TurretBlueprint.UnitType.Turret)
        {
            node.turretOnThisNode.GetComponent<TurretActivation>().unitType = TurretActivation.UnitType.Turret;
            UnitManager.instance.RegisterTurret(node.turretOnThisNode);
        }
        else
        {
            node.turretOnThisNode.GetComponent<TurretActivation>().unitType = TurretActivation.UnitType.Building;
            UnitManager.instance.RegisterBuilding(node.turretOnThisNode);
        }
    }


    private static void RemoveUnitsFromUnitManagerAndReloadBuildings(Node node, TurretBlueprint blueprintToUpgrade)
    {
        if (blueprintToUpgrade.unitType == TurretBlueprint.UnitType.Turret)
            UnitManager.instance.RemoveTurret(node.turretOnThisNode);
        else
            UnitManager.instance.RemoveBuilding(node.turretOnThisNode);

        DestroyImmediate(node.turretOnThisNode);

        if (blueprintToUpgrade.unitType == TurretBlueprint.UnitType.Building && !blueprintToUpgrade.turret.GetComponent<Unit_Stats>().isWorkshop)
        {
            UnitManager.instance.ReloadBuildingsIfAny();
        }



    }











    public void SpawnUnitPrefabsOnNode(Node node)
    {
        for(int i = 0; i < unitToBuild.prefabsToSpawnOnInstantiate.Length; i++)
        {
            Instantiate(unitToBuild.prefabsToSpawnOnInstantiate[i], node.GetBuildPosition(), Quaternion.identity);
        }
    }

    public void SpawnUpgradePrefabsOnNode(Node node)
    {
        for (int i = 0; i < node.blueprintToUpgrade.listOfUpgrades[node.currentUpgradeVersion].prefabsToSpawnOnUpgrade.Length; i++)
        {
            Instantiate(node.blueprintToUpgrade.listOfUpgrades[node.currentUpgradeVersion].prefabsToSpawnOnUpgrade[i], node.GetBuildPosition(), Quaternion.identity);
        }
    }

}
