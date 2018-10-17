using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TurretBlueprint {
    
    public string tag;
    public int workShopLevel;
    public enum UnitType {Turret, Building};
    public UnitType unitType;
    public GameObject turret;
    public int cost;
    public int sellIncome;
    public float buildDelay;
    public Button associatedButton;
    public Transform[] prefabsToSpawnOnInstantiate;
    public UpgradedTurrets[] listOfUpgrades;





    [System.Serializable]
    public class UpgradedTurrets
    {
        public string tag;
        public int workShopLevel;
        public GameObject upgradeTurret;
        public int upgradeCost;
        public int upgradeSellIncome;
        public Transform[] prefabsToSpawnOnUpgrade;
    }
}

