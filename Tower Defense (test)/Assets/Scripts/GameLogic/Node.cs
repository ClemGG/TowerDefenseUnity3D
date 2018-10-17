using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    public TurretBlueprint unitToPlaceOnAwake;

    [Space(10)]

    public Color canBuildColor;
    public Color cantBuildColor;
    public bool isPoweredByGenerator = false;
    public bool isAffectedBySpotlight = false;
    




    [HideInInspector] public GameObject turretOnThisNode;
    [HideInInspector] public int currentUpgradeVersion;
    [HideInInspector] public TurretBlueprint blueprintToUpgrade;

    private Vector3 positionOffset;

    private Renderer rend;
    private Color startColor;


    public Color nodeVisibleColor;
    public Color nodeInvisibleColor;
    public bool isVisible = false;

    TurretBuilder turretBuilder;






    private void Start()
    {


        currentUpgradeVersion = 0;

        turretBuilder = TurretBuilder.instance;
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;


        if (unitToPlaceOnAwake.turret != null)
        {
            turretBuilder.Set_Unit_To_Build(unitToPlaceOnAwake);
            blueprintToUpgrade = unitToPlaceOnAwake;
            turretBuilder.BuildUnitOnNode(this, blueprintToUpgrade);
        }

    }




    public Vector3 GetBuildPosition()
    {
        return transform.position + new Vector3(0f, 0.5f, 0f);
    }




    private void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;



        if (turretOnThisNode != null)
        {

            if (turretOnThisNode.GetComponent<Unit_Stats>().isWorkshop)
            {
                turretOnThisNode.GetComponent<TurretActivation>().isNotBuiltYet = false;
            }

            if (turretBuilder.HasAnUnitToBuild)
            {
                Msg.instance.Print(Msg.ErrorType.Error,25, "Une unité est déjà présente sur cette case.", true, true);
                turretBuilder.GetComponent<AudioSource>().Play();

            }
        }




         if (turretOnThisNode != null && !turretOnThisNode.GetComponent<TurretActivation>().isNotBuiltYet)
         {

            

            

            if (turretOnThisNode.GetComponent<Unit_Stats>().isWorkshop)
            {
                turretOnThisNode.GetComponent<TurretActivation>().isNotBuiltYet = false;
                turretBuilder.nodeUI.DisableSellButton(35, "Vous ne pouvez ps vendre cette unité");
            }
            else
            {
                turretBuilder.nodeUI.EnableSellButton();
            }


            if (blueprintToUpgrade.listOfUpgrades.Length == 0)
            {
                //Debug.Log("No upgrades for " + blueprintToUpgrade.turret.name);
                turretBuilder.nodeUI.DisableUpgradeButton(35, "Aucune amélioration pour " + blueprintToUpgrade.turret.name);
            }
            else
            {
                if (currentUpgradeVersion < blueprintToUpgrade.listOfUpgrades.Length)
                {
                    if (PlayerStats.instance.currentMoney < blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeCost)
                    {
                        //Debug.Log("Not enough money to upgrade " + blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.name);
                        turretBuilder.nodeUI.DisableUpgradeButton(30, "Pas assez d'argent pour acheter l'upgrade " + blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.name);
                    }
                    else
                    {
                        if (turretOnThisNode.GetComponent<Unit_Stats>().isDisabledByEvent)
                        {
                            turretBuilder.nodeUI.DisableUpgradeButton(30, "Vous ne pouvez pas améliorer les unités désactivées.");
                        }
                        else
                        {
                            if(TurretBuilder.atelierInstance != null)
                            {
                                if(currentUpgradeVersion <= TurretBuilder.instance.currentWorkshopLevel)
                                {
                                    turretBuilder.nodeUI.EnableUpgradeButton();
                                }
                                else
                                {
                                    turretBuilder.nodeUI.DisableUpgradeButton(30, "Vous devez améliorer l'Atelier au niveau " + (blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].workShopLevel+1).ToString());
                                }
                            }
                            else
                            {

                                if (currentUpgradeVersion <= 0)
                                {
                                    turretBuilder.nodeUI.EnableUpgradeButton();
                                }
                                else
                                {
                                    if (PlayerStats.instance.currentMoney < blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeCost)
                                    {
                                        turretBuilder.nodeUI.DisableUpgradeButton(30, "Pas assez d'argent pour acheter l'upgrade " + blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.name);
                                    }
                                    else
                                    {
                                        turretBuilder.nodeUI.DisableUpgradeButton(30, "Vous devez posséder un Atelier pour continuer d'améliorer cette tourelle.");

                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    //Debug.Log("No upgrades for " + blueprintToUpgrade.listOfUpgrades[currentUpgradeVersion].upgradeTurret.name);
                    turretBuilder.nodeUI.DisableUpgradeButton(35, "Aucune amélioration pour " + blueprintToUpgrade.turret.name);
                }
            }

            turretBuilder.SelectNode(this);
            return;
         }


        turretBuilder.DeselectNode();



        if (!turretBuilder.HasAnUnitToBuild)
            return;




        if (!turretBuilder.IsTurretEligibleForBuildOnNode(this, turretBuilder.Get_Unit_To_Build()))
        {
            Msg.instance.Print(Msg.ErrorType.Error, 25, "Aucun générateur n'est à proximité.", true, true);
            turretBuilder.GetComponent<AudioSource>().Play();
            return;
        }


        //Build a turret
        blueprintToUpgrade = turretBuilder.Get_Unit_To_Build();
        turretBuilder.BuildUnitOnNode(this, blueprintToUpgrade);

    }








    private void OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (turretBuilder.HasAnUnitToBuild)
        {
            if (turretOnThisNode != null || !turretBuilder.HasEnoughMoneyToBuildNextUnit || !turretBuilder.IsTurretEligibleForBuildOnNode(this, turretBuilder.Get_Unit_To_Build()))
            {
                rend.material.color = cantBuildColor;
            }
            else if (turretOnThisNode == null && turretBuilder.HasEnoughMoneyToBuildNextUnit && turretBuilder.IsTurretEligibleForBuildOnNode(this, turretBuilder.Get_Unit_To_Build()))
            {
                rend.material.color = canBuildColor;
            }
        }


        if (turretOnThisNode != null && !turretOnThisNode.GetComponent<Unit_Stats>().isDisabledByEvent)
        {
            turretOnThisNode.GetComponent<Unit_Stats>().sphereRange.SetActive(true);
        }


        if(Shop.instance.preview != null)
        {

            if (turretOnThisNode == null)
            {
                Shop.instance.preview.SetActive(true);
                Shop.instance.preview.transform.position = GetBuildPosition();
            }
            else
            {
                Shop.instance.preview.SetActive(false);
            }

        }

    }






    private void OnMouseExit()
    {
        if (isVisible)
        {
            rend.material.color = nodeVisibleColor;
        }
        else
        {
            rend.material.color = nodeInvisibleColor;
        }


        if (turretOnThisNode != null)
        {
            turretOnThisNode.GetComponent<Unit_Stats>().sphereRange.SetActive(false);
        }



        if (Shop.instance.preview != null)
        {
            Shop.instance.preview.SetActive(false);
        }
    }


}

