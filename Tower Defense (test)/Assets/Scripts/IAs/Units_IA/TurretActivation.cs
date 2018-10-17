using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretActivation : MonoBehaviour {

    [HideInInspector] public Node linkedNode;
    [HideInInspector] public bool isAnUpgrade;
    [HideInInspector] public bool isNotBuiltYet;


    public enum UnitType { Turret, Building };
    public UnitType unitType;

    public Transform delayCanvas;

    public Material newMaterialToAssign;
    public Material snowMaterial;
    public bool AssignNewMaterialOnSpawn;

    public float snowDelay = 30f;
    public float snowShaderMinValue = 1.3f;
    public float snowShaderMaxValue = 2.15f;
    public float snowCoverSpeed = 0.001f;
    public float BlizzardEventCoverSpeed = 15f;

    [Space(10)]

    public MeshRenderer[] turretRenderers;
    public MonoBehaviour[] scriptsToActivate;

    // Use this for initialization
    void Start () {

        GetComponent<Unit_Stats>().UpdateSphereRadius();
        GetComponent<Unit_Stats>().sphereRange.SetActive(false);

        if (!isAnUpgrade)
        {
            isNotBuiltYet = true;

            if (!GetComponent<Unit_Stats>().isWorkshop)
            {
                foreach (MonoBehaviour script in scriptsToActivate)
                {
                    script.enabled = false;
                }
            }
        }
        else
        {
           delayCanvas.gameObject.SetActive(false);
            
            isNotBuiltYet = false;

            AssignSnowShader(false);
           EnableScriptsIfPowered();
           DisableScriptsAtNightIfTurret();
           EnableScriptsAtNightIfTurretFromSpotlight();
        }

    }





    public IEnumerator WaitBeforeCreatingNewTurret(float buildDelay)
    {

        
        delayCanvas.gameObject.SetActive(true);

        float timer = 0f;
        Image delayRadial = delayCanvas.GetChild(1).GetComponent<Image>();
        Text countdownText = delayCanvas.GetChild(2).GetComponent<Text>();

        while (timer < buildDelay)
        {
            timer += Time.deltaTime;
            delayRadial.fillAmount = 1 - timer / buildDelay;
            countdownText.text = Mathf.Ceil(buildDelay - timer).ToString();


            if (turretRenderers != null)
            {
                foreach (MeshRenderer rend in turretRenderers)
                {
                    if (rend)
                        rend.material.SetFloat("_DissolveIntensity", 1 - timer / buildDelay);
                }
            }

            yield return null;
        }


        if (AssignNewMaterialOnSpawn)
        {
            AssignNewShader();
        }
        else
        {
            AssignSnowShader(false);
        }

        delayCanvas.gameObject.SetActive(false);

        isNotBuiltYet = false;



        PowerLinkedNodeIfGenerator(); // Permet au générateur d'activer ses scripts à sa construction (car sa Node n'est pas alimentée de base).
        EnableScriptsIfPowered(); // Active tous les scripts de l'unité une fois construite si elle est alimentée.
        DisableScriptsIfNotPowered(); // Si le générateur est détruit avant que la tourelle ait fini de se construire, on désactive les scripts.
        DisableScriptsAtNightIfTurret(); // Désactive automatiquement la tourelle si cette dernière est construite de nuit
        EnableScriptsAtNightIfTurretFromSpotlight(); //Active la tourelle de nuit si celle-ci est proche d'une spotlight
        EnableScriptsAtNightIfSpotlight(); // Active la spotlight de nuit
        DisableScriptsAtDayIfSpotlight(); // Désactive la spotlight de jour


        
    }


    public IEnumerator UpdateSnowEffect(MeshRenderer rend, bool usedByEvent)
    {
        rend.material.SetFloat("_CoverLevel", 0f); // on le reset à 0 de base par précaution

        float timer = 0f;
        


        if (usedByEvent)
        {
            //rend.material.SetFloat("_CoverLevel", snowShaderMaxValue);
            //yield return null;

            while (rend.material.GetFloat("_CoverLevel") < snowShaderMaxValue)
            {
                rend.material.SetFloat("_CoverLevel", rend.material.GetFloat("_CoverLevel") + BlizzardEventCoverSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {

            while (timer <= snowDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            while (timer > snowDelay && rend.material.GetFloat("_CoverLevel") < snowShaderMinValue)
            {
                rend.material.SetFloat("_CoverLevel", rend.material.GetFloat("_CoverLevel") + snowCoverSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }


    }







    public void AssignNewShader()
    {
        if (newMaterialToAssign != null)
        {
            foreach (MeshRenderer rend in turretRenderers)
            {
                if (rend)
                    rend.material = newMaterialToAssign;
            }
        }
    }
    public void AssignSnowShader(bool usedByEvent)
    {
        if (snowMaterial != null)
        {
            foreach (MeshRenderer rend in turretRenderers)
            {
                if (rend)
                {
                    rend.material = snowMaterial;
                    StartCoroutine(UpdateSnowEffect(rend, usedByEvent));
                }
            }

        }
    }












    public void PowerLinkedNodeIfGenerator()
    {

        //Si l'unité est alimentée, alors elle peut activer son IA.
        if (unitType == UnitType.Building)
        {
            if (GetComponent<Unit_Stats>().isGenerator)
            {
                linkedNode.isPoweredByGenerator = true;
            }
        }

    }
    public void ShutDownLinkedNodeIfGenerator()
    {

        //Si l'unité est alimentée, alors elle peut activer son IA.
        if (unitType == UnitType.Building)
        {
            if (GetComponent<Unit_Stats>().isGenerator)
            {
                linkedNode.isPoweredByGenerator = false;
            }
        }

    }









    public void EnableScriptsIfPowered()
    {

        if (GetComponent<Unit_Stats>().isWorkshop)
        {
            foreach (MonoBehaviour script in scriptsToActivate)
            {
                script.enabled = true;
            }

            return;
        }

            //Si l'unité est alimentée, alors elle peut activer son IA.
            if (linkedNode.isPoweredByGenerator)
            {
                foreach (MonoBehaviour script in scriptsToActivate)
                {
                    script.enabled = true;
                }
                
            }


    }
    public void DisableScriptsIfNotPowered()
    {


        if (GetComponent<Unit_Stats>().isWorkshop)
        {
            return;
        }
        

        //Sinon, l'unité ne fait rien tant qu'elle n'est pas à nouveau alimentée.
        if (!linkedNode.isPoweredByGenerator)
        {
            foreach (MonoBehaviour script in scriptsToActivate)
            {
                script.enabled = false;
            }

            GetComponent<Unit_Stats>().sphereRange.SetActive(false);
        }


    }











    public void DisableScriptsAtNightIfTurret()
    {


        if (GetComponent<Unit_Stats>().isWorkshop)
        {
            return;
        }

        //Et si l'unité est une tourelle, et qu'elle est construite de nuit, elle se désactive de toute façon (Elle sera réactivée automatiquement).
        if (!DayNightSystem.instance.isDay && unitType == UnitType.Turret)
        {
            if (!linkedNode.isAffectedBySpotlight)
            {
                foreach (MonoBehaviour script in scriptsToActivate)
                {
                    script.enabled = false;
                }
            }
            GetComponent<Unit_Stats>().sphereRange.SetActive(false);
        }

        //print(DayNightSystem.instance.isDay);
    }
    public void EnableScriptsAtNightIfTurretFromSpotlight()
    {


        if (GetComponent<Unit_Stats>().isWorkshop)
        {
            foreach (MonoBehaviour script in scriptsToActivate)
            {
                script.enabled = true;
            }
            return;
        }


        //Et si l'unité est une tourelle, et qu'elle est construite de nuit, elle se désactive de toute façon (Elle sera réactivée automatiquement).
        if (!DayNightSystem.instance.isDay && unitType == UnitType.Turret)
        {
            if (linkedNode.isAffectedBySpotlight && linkedNode.isPoweredByGenerator)
            {
                foreach (MonoBehaviour script in scriptsToActivate)
                {
                    script.enabled = true;
                }
            }
            GetComponent<Unit_Stats>().sphereRange.SetActive(false);
        }

        //print(DayNightSystem.instance.isDay);
    }









    public void DisableScriptsDuringBlizzardEventIfGenerator()
    {
        foreach (MonoBehaviour script in scriptsToActivate)
        {
            script.enabled = false;
        }

        GetComponent<Unit_Stats>().sphereRange.SetActive(false);
    }













    public void EnableScriptsAtNightIfSpotlight()
    {
        //Si l'unité est une Spotlight, et qu'elle est construite de nuit, elle s'active automatiquement.
        
        if (!DayNightSystem.instance.isDay && 
            unitType == UnitType.Building && 
            GetComponent<Unit_Stats>().isSpotlight && 
            linkedNode.isPoweredByGenerator)
        {
            foreach (MonoBehaviour script in scriptsToActivate)
            {
                script.enabled = true;
            }

            GetComponent<Unit_Stats>().sphereRange.SetActive(false);
            GetComponent<Spotlight_IA>().ToggleLights(true);
            GetComponent<Spotlight_IA>().ActivateNearbyNodes();
            StopAllCoroutines();
            AssignNewShader();
        }

        //print(DayNightSystem.instance.isDay);
    }
    public void DisableScriptsAtDayIfSpotlight()
    {
        //Et si l'unité est une Spotlight, et qu'elle est construite de nuit, elle s'active automatiquement.





        if (DayNightSystem.instance.isDay &&
            unitType == UnitType.Building &&
            GetComponent<Unit_Stats>().isSpotlight)
        {

            GetComponent<Unit_Stats>().sphereRange.SetActive(false);
            GetComponent<Spotlight_IA>().ToggleLights(false);
            AssignSnowShader(false);

            foreach (MonoBehaviour script in scriptsToActivate)
            {
                script.enabled = false;
            }

        }

        //print(DayNightSystem.instance.isDay);
    }











    public void DisableSpotlightFromGenerator()
    {
        GetComponent<Spotlight_IA>().ToggleLights(false);
        GetComponent<Spotlight_IA>().RevertNodeState();

        foreach (MonoBehaviour script in scriptsToActivate)
        {
            script.enabled = false;
        }

        GetComponent<Unit_Stats>().sphereRange.SetActive(false);
    }
}
