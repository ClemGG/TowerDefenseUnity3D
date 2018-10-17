using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUi : MonoBehaviour {

    [HideInInspector] public Node targetedNode;
    public Text unitName;
    public Button upgradeButton;
    public Button sellButton;

    public Vector3 uiPosition = new Vector3(-9.2f, 5f, 0f);

    public void SetTarget(Node node)
    {
        unitName.text = node.turretOnThisNode.name.Replace("(Clone)", null);
        targetedNode = node;
        transform.position = GetUiPosition();
        transform.Find("Canvas").gameObject.SetActive(true);
        UpdateUpgradeAndSellCosts();
    }

    public Vector3 GetUiPosition()
    {
        return targetedNode.GetBuildPosition() + uiPosition;
    }

    public void HideUI()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }




    public void UpdateUpgradeAndSellCosts()
    {
        if(targetedNode.blueprintToUpgrade.listOfUpgrades.Length > 0 && targetedNode.currentUpgradeVersion < targetedNode.blueprintToUpgrade.listOfUpgrades.Length)
            upgradeButton.transform.GetChild(1).GetComponent<Text>().text = "-" + targetedNode.blueprintToUpgrade.listOfUpgrades[targetedNode.currentUpgradeVersion].upgradeCost.ToString();

        if(targetedNode.currentUpgradeVersion == 0)
            sellButton.transform.GetChild(1).GetComponent<Text>().text = "+" + targetedNode.blueprintToUpgrade.sellIncome.ToString();
        else
            sellButton.transform.GetChild(1).GetComponent<Text>().text = "+" + targetedNode.blueprintToUpgrade.listOfUpgrades[targetedNode.currentUpgradeVersion-1].upgradeSellIncome.ToString();
    }



    public void UpgradeTurret()
    {
        TurretBuilder.instance.UpgradeUnitOnNode(targetedNode, ref targetedNode.currentUpgradeVersion, targetedNode.blueprintToUpgrade);
        TurretBuilder.instance.DeselectNode();
    }

    public void SellTurret()
    {
        TurretBuilder.instance.SellUnitOnNode(targetedNode, ref targetedNode.currentUpgradeVersion, targetedNode.blueprintToUpgrade);
        TurretBuilder.instance.DeselectNode();
    }







    public void DisableUpgradeButton(int fontSize, string msg)
    {
        upgradeButton.transform.GetChild(0).gameObject.SetActive(false);
        upgradeButton.transform.GetChild(1).gameObject.SetActive(false);
        
        upgradeButton.transform.GetChild(2).gameObject.SetActive(true);
        upgradeButton.transform.GetChild(2).GetComponent<Text>().fontSize = fontSize;
        upgradeButton.transform.GetChild(2).GetComponent<Text>().text = msg;

        upgradeButton.transform.GetChild(3).gameObject.SetActive(false);
        upgradeButton.interactable = false;
    }
    public void EnableUpgradeButton()
    {
        upgradeButton.transform.GetChild(0).gameObject.SetActive(true);
        upgradeButton.transform.GetChild(1).gameObject.SetActive(true);
        upgradeButton.transform.GetChild(2).gameObject.SetActive(false);
        upgradeButton.transform.GetChild(3).gameObject.SetActive(true);

        upgradeButton.interactable = true;
    }





    public void DisableSellButton(int fontSize, string msg)
    {
        sellButton.transform.GetChild(0).gameObject.SetActive(false);
        sellButton.transform.GetChild(1).gameObject.SetActive(false);
        
        sellButton.transform.GetChild(2).gameObject.SetActive(true);
        sellButton.transform.GetChild(2).GetComponent<Text>().fontSize = fontSize;
        sellButton.transform.GetChild(2).GetComponent<Text>().text = msg;

        sellButton.transform.GetChild(3).gameObject.SetActive(false);
        sellButton.interactable = false;
    }
    public void EnableSellButton()
    {
        sellButton.transform.GetChild(0).gameObject.SetActive(true);
        sellButton.transform.GetChild(1).gameObject.SetActive(true);
        sellButton.transform.GetChild(2).gameObject.SetActive(false);
        sellButton.transform.GetChild(3).gameObject.SetActive(true);
        sellButton.interactable = true;
    }
}
