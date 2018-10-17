using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour {


    [Header("Preview : ")]
    public GameObject preview;

    [Space(10)]

    public RectTransform turretContent;
    public RectTransform buildingContent;
    public Color colorTurretAttributeDisabled;

    [Space(10)]

    [Header("Nodes : ")]
    public Node[] allNodesInScene;

    [Space(10)]

    [Header("Shop lists : ")]
    public TurretBlueprint[] availableTurrets;
    [Space(10)]
    public TurretBlueprint[] availableBuildings;



    public static Shop instance;


    private void Awake()
    {
        if (instance != null)
        {
            print("More than one Shop in scene !");
            return;
        }

        instance = this;

    }

    


    private void Start()
    {
        allNodesInScene = FindObjectsOfType<Node>();
        ToggleNodesVisibility(false);

        Set_Cost_For_All_Units();
        Set_BuildDelay_For_All_Units();

        Set_Icons_For_All_Buttons();

        Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney();

        Set_LockState_For_All_Buttons(0);
        
    }














    public void Set_LockState_For_All_Buttons(int currentWorkshopLevel)
    {
        foreach (TurretBlueprint turret in availableTurrets)
        {


            if (turret.workShopLevel <= currentWorkshopLevel)
            {
                turret.associatedButton.transform.Find("LockImage").gameObject.SetActive(false);
                turret.associatedButton.interactable = true;
            }
            else
            {
                turret.associatedButton.transform.Find("LockImage").gameObject.SetActive(true);
                turret.associatedButton.interactable = false;
            }
               

        }

        foreach (TurretBlueprint building in availableBuildings)
        {
            if (building.workShopLevel <= currentWorkshopLevel)
            {
                building.associatedButton.transform.Find("LockImage").gameObject.SetActive(false);
                building.associatedButton.interactable = true;
            }
            else
            {
                building.associatedButton.transform.Find("LockImage").gameObject.SetActive(true);
                building.associatedButton.interactable = false;
            }

        }
    }
    

    private void Set_Icons_For_All_Buttons()
    {
        foreach (TurretBlueprint turret in availableTurrets)
        {
            Unit_Stats unitStats = turret.turret.gameObject.GetComponent<Unit_Stats>();

            if (!unitStats.CanHitFlyingEnemies)
                turret.associatedButton.transform.GetChild(3).Find("Wing Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;
            if (!unitStats.CanUseAOE)
                turret.associatedButton.transform.GetChild(3).Find("AOE Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;
            if (!unitStats.CanSlowDownEnemies)
                turret.associatedButton.transform.GetChild(3).Find("Slow Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;

        }

        foreach (TurretBlueprint building in availableBuildings)
        {
            Unit_Stats unitStats = building.turret.gameObject.GetComponent<Unit_Stats>();

            if (!unitStats.isGenerator)
                building.associatedButton.transform.GetChild(3).Find("Generator Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;
            if (!unitStats.isSpotlight)
                building.associatedButton.transform.GetChild(3).Find("Spotlight Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;
            if (!unitStats.isTargetForEnemies)
                building.associatedButton.transform.GetChild(3).Find("Target Icon".Trim()).GetComponent<Image>().color = colorTurretAttributeDisabled;

        }
    }


    


    public void Disable_Unit_Buttons()
    {

        foreach (TurretBlueprint turret in availableTurrets)
        {
            turret.associatedButton.interactable = false;
        }

        foreach (TurretBlueprint building in availableBuildings)
        {
            building.associatedButton.interactable = false;
        }
    }
    public void Enable_Unit_Buttons()
    {

        foreach (TurretBlueprint turret in availableTurrets)
        {
            if (PlayerStats.instance.currentMoney >= turret.cost)
            {
                turret.associatedButton.interactable = true;
            }
        }

        foreach (TurretBlueprint building in availableBuildings)
        {
            if (PlayerStats.instance.currentMoney >= building.cost)
            {
                building.associatedButton.interactable = true;
            }
        }
    }






    public void Set_Cost_For_All_Units()
    {
        for (int i = 0; i < availableTurrets.Length; i++)
        {
            turretContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableTurrets[i].cost.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            buildingContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableBuildings[i].cost.ToString();
        }
    }
    public void Set_BuildDelay_For_All_Units()
    {
        for (int i = 0; i < availableTurrets.Length; i++)
        {
            turretContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableTurrets[i].buildDelay.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            buildingContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableBuildings[i].buildDelay.ToString();
        }
    }











    #region En rapport avec l'event Magma

    public void Set_Cost_For_All_Units_On_Magma_Event(int coefReduction)
    {
        for (int i = 0; i < availableTurrets.Length; i++)
        {
            availableTurrets[i].cost /= coefReduction;
            turretContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableTurrets[i].cost.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            availableBuildings[i].cost /= coefReduction;
            buildingContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableBuildings[i].cost.ToString();
        }

        Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney();
    }




    public void Set_BuildDelay_For_All_Units_On_Magma_Event(int coefReduction)
    {
        for (int i = 0; i < availableTurrets.Length; i++)
        {
            availableTurrets[i].buildDelay /= coefReduction;
            turretContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableTurrets[i].buildDelay.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            availableBuildings[i].buildDelay /= coefReduction;
            buildingContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableBuildings[i].buildDelay.ToString();
        }
    }




    public void Revert_Magma_Event(int coefReduction)
    {
        for (int i = 0; i < availableTurrets.Length; i++)
        {
            availableTurrets[i].cost *= coefReduction;
            turretContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableTurrets[i].cost.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            availableBuildings[i].cost *= coefReduction;
            buildingContent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = availableBuildings[i].cost.ToString();
        }

        Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney();


        for (int i = 0; i < availableTurrets.Length; i++)
        {
            availableTurrets[i].buildDelay *= coefReduction;
            turretContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableTurrets[i].buildDelay.ToString();
        }

        for (int i = 0; i < availableBuildings.Length; i++)
        {
            availableBuildings[i].buildDelay *= coefReduction;
            buildingContent.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = availableBuildings[i].buildDelay.ToString();
        }
    }


    #endregion










    public void Disable_Or_Enable_Unit_Buttons_Depending_Of_CurrentMoney()
    {
        foreach (TurretBlueprint turret in availableTurrets)
        {
            if (PlayerStats.instance.currentMoney < turret.cost || turret.workShopLevel > TurretBuilder.instance.currentWorkshopLevel)
            {
                turret.associatedButton.interactable = false;
            }
            if(PlayerStats.instance.currentMoney >=  turret.cost && turret.workShopLevel <= TurretBuilder.instance.currentWorkshopLevel)
            {
                turret.associatedButton.interactable = true;
            }
        }

        foreach (TurretBlueprint building in availableBuildings)
        {
            if (PlayerStats.instance.currentMoney < building.cost || building.workShopLevel > TurretBuilder.instance.currentWorkshopLevel)
            {
                building.associatedButton.interactable = false;
            }
            if (PlayerStats.instance.currentMoney >= building.cost && building.workShopLevel <= TurretBuilder.instance.currentWorkshopLevel)
            {
                building.associatedButton.interactable = true;
            }
        }
    }




    #region Buildings
    

    public void PurchaseBuilding(int index)
    {


        DestroyPreview();


        //Debug.Log(availableBuildings[index].turret.name + " selected.");

        //Pour cacher le pop-up Upgrade & Sell
        TurretBuilder.instance.DeselectNode();

        if (TurretBuilder.instance.HasAnUnitToBuild && TurretBuilder.instance.Get_Unit_To_Build().turret.name == availableBuildings[index].turret.name)
        {
            TurretBuilder.instance.Set_Unit_To_Build(null);
            ToggleNodesVisibility(false);

            foreach (TurretBlueprint tb in availableBuildings)
            {
                tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
            }

        }
        else if (TurretBuilder.instance.HasAnUnitToBuild && TurretBuilder.instance.Get_Unit_To_Build().turret.name != availableBuildings[index].turret.name)
        {
            TurretBuilder.instance.Set_Unit_To_Build(availableBuildings[index]);
            ToggleNodesVisibility(true);

            foreach (TurretBlueprint tb in availableBuildings)
            {
                tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
            }
            
            availableBuildings[index].associatedButton.GetComponent<ReadyButton>().Ready_ColorBlocks();



            AssignNewPreview(availableBuildings[index].turret);

        }
        else
        {
            TurretBuilder.instance.Set_Unit_To_Build(availableBuildings[index]);
            ToggleNodesVisibility(true);

            
            availableBuildings[index].associatedButton.GetComponent<ReadyButton>().Ready_ColorBlocks();

            AssignNewPreview(availableBuildings[index].turret);

        }


        foreach (TurretBlueprint tb in availableTurrets)
        {
            tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
        }


    }
    #endregion


    
    #region Turrets
    
    
    public void PurchaseTurret(int index)
    {
        DestroyPreview();





        //Debug.Log(availableTurrets[index].turret.name + " selected.");

        //Pour cacher le pop-up Upgrade & Sell
        TurretBuilder.instance.DeselectNode();

        if (TurretBuilder.instance.HasAnUnitToBuild && TurretBuilder.instance.Get_Unit_To_Build().turret.name == availableTurrets[index].turret.name)
        {
            TurretBuilder.instance.Set_Unit_To_Build(null);
            ToggleNodesVisibility(false);


            foreach (TurretBlueprint tb in availableTurrets)
            {
                tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
            }

        }
        else if (TurretBuilder.instance.HasAnUnitToBuild && TurretBuilder.instance.Get_Unit_To_Build().turret.name != availableTurrets[index].turret.name)
        {
            TurretBuilder.instance.Set_Unit_To_Build(availableTurrets[index]);
            ToggleNodesVisibility(true);

            foreach (TurretBlueprint tb in availableTurrets)
            {
                tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
            }
            
            availableTurrets[index].associatedButton.GetComponent<ReadyButton>().Ready_ColorBlocks();


            AssignNewPreview(availableTurrets[index].turret);

        }
        else
        {
            TurretBuilder.instance.Set_Unit_To_Build(availableTurrets[index]);
            ToggleNodesVisibility(true);
            
            availableTurrets[index].associatedButton.GetComponent<ReadyButton>().Ready_ColorBlocks();


            AssignNewPreview(availableTurrets[index].turret);

        }



        foreach (TurretBlueprint tb in availableBuildings)
        {
            tb.associatedButton.GetComponent<ReadyButton>().Start_ColorBlocks();
        }

    }




    #endregion





    public void AssignNewPreview(GameObject unit)
    {
        preview = Instantiate(unit, transform.position, unit.transform.localRotation);
        foreach (MonoBehaviour script in preview.GetComponents(typeof(MonoBehaviour)))
        {
            script.enabled = false;
        }
        preview.SetActive(false);
    }

    public void DestroyPreview()
    {
        if (preview != null)
        {
            DestroyImmediate(preview);
        }
    }



    public void ToggleNodesVisibility(bool showNodes)
    {
        foreach (Node node in allNodesInScene)
        {
            if (showNodes)
            {
                node.isVisible = true;
                node.GetComponent<MeshRenderer>().material.color = node.nodeVisibleColor;
            }
            else
            {
                node.isVisible = false;
                node.GetComponent<MeshRenderer>().material.color = node.nodeInvisibleColor;
            }
        }
    }
}
