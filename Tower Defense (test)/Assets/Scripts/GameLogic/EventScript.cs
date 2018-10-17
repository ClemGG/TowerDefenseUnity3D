
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventScript : MonoBehaviour {
    
    [Header("Chance Variables :")]

    [Range (0f,1f)]
    public float disableChance = 0.5f;
    [Range (1f,1.5f)]
    public float coefRange = 1.5f;



    [Range (1,10)]
    public int coefCostBuildReduction = 2;
    [Range(1f, 1.5f)]
    public float coefFireRate = 1.5f;


    [Header("Event :")]

    private float delayBeforeEvent;
    private int startNbOfWaves;
    public EventToPlay currentEvent;


    [Header("ParticleSystem :")]

    public ParticleSystem snowSystem;
    public float normalSpeed = 15f;
    public float normalRate = 70f;
    public float normalParticleSize = 0.0007f;

    public float blizzardSpeed = 35f;
    public float blizzardRate = 350f;
    public float blizzardParticleSize = 0.0015f;



    [Header("AudioSource :")]

    public float blizzardFadeSpeed = 2f;
    public AudioSource BlizzardSound;
    public AudioSource MagmaSound;





    public static EventScript instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Msg.instance.Print(Msg.ErrorType.Error, 25, "More than one EventScript in scene !", true, false);
            return;
        }

        instance = this;
        
    }













    public void PlayCorrespondingEffect(EventToPlay newEvent)
    {
        currentEvent = newEvent;
        delayBeforeEvent = currentEvent.delayBeforeEventActivation;
        startNbOfWaves = currentEvent.nbOfWaves;


        switch (currentEvent.eventName)
        {
            case "Blizzard":
                StartCoroutine(WaitBeforeStartingBlizzardEvent(delayBeforeEvent));
                StartCoroutine(PlayBlizzardSound(true));
                break;

            case "Magma":
                StartCoroutine(WaitBeforeStartingMagmaEvent(delayBeforeEvent));
                StartCoroutine(PlayMagmaSound());

                break;

            default:
                Msg.instance.Print(Msg.ErrorType.Info, 25, "L'event \"" + currentEvent.eventName+"\" n'a pas de fonction associée.", true, false);
                break;
            
        }
    }




    #region Toutes les fonctions relatives aux events


    #region Blizzard

    private void BlizzardEvent()
    {
        UnitManager.instance.isEventBlizzard = true;
        UnitManager.instance.DisableRandomUnitsOnBlizzardEvent(disableChance, coefRange);
        
    }

    private void RevertBlizzard()
    {
        UnitManager.instance.isEventBlizzard = false;
        UnitManager.instance.RevertBlizzardEvent(coefRange);

        StartCoroutine(PlayBlizzardSound(false));
    }

    private IEnumerator WaitBeforeStartingBlizzardEvent(float delayBeforeEvent)
    {
        yield return new WaitForSeconds(delayBeforeEvent);
        Msg.instance.Print(Msg.ErrorType.Info, 21, "Evénement : <color=#90ddf8>" + currentEvent.eventName + "</color>. Vos tourelles ont une chance d'être désactivées.", false, true);
        BlizzardEvent();
        DisableCurrentEventAfterNumberOfWavesPassed(false);
    }

    private IEnumerator PlayBlizzardSound(bool onEventStarted)
    {
        bool stop = false;

        while (!stop)
        {

            if (onEventStarted)
            {
                if (BlizzardSound.volume < 1f)
                {
                    BlizzardSound.volume += Time.deltaTime / blizzardFadeSpeed;
                }
                if (BlizzardSound.pitch <= 1.85f)
                {
                    BlizzardSound.pitch += Time.deltaTime / blizzardFadeSpeed;
                }

                if(Mathf.Approximately(BlizzardSound.volume, 1f) && BlizzardSound.pitch >= 1.85f)
                {
                    stop = true;
                }
            }
            else
            {
                if (BlizzardSound.volume > .4f)
                {
                    BlizzardSound.volume -= Time.deltaTime / blizzardFadeSpeed;
                }
                if ( BlizzardSound.pitch >= .85f)
                {
                    BlizzardSound.pitch -= Time.deltaTime / blizzardFadeSpeed;
                }

                if (BlizzardSound.volume <= .4f && BlizzardSound.pitch <= .85f)
                {
                    stop = true;
                }

            }
                    yield return null;
        }



    }

    #endregion








    #region Magma

    private void MagmaEvent()
    {
        UnitManager.instance.isEventMagma = true;

        Shop.instance.Set_Cost_For_All_Units_On_Magma_Event(coefCostBuildReduction);
        Shop.instance.Set_BuildDelay_For_All_Units_On_Magma_Event(coefCostBuildReduction);

        UnitManager.instance.BoostUnitsOnMagmaEvent(coefFireRate, coefRange);
    }

    private void RevertMagma()
    {
        UnitManager.instance.isEventMagma = false;

        Shop.instance.Revert_Magma_Event(coefCostBuildReduction);
        UnitManager.instance.RevertMagmaEvent(coefFireRate, coefRange);
    }

    private IEnumerator WaitBeforeStartingMagmaEvent(float delayBeforeEvent)
    {
        yield return new WaitForSeconds(delayBeforeEvent);
        Msg.instance.Print(Msg.ErrorType.Info, 21, "Evénement : <color=#d93907>" + currentEvent.eventName + "</color>. Vos tourelles coûtent moins de ressources et sont plus rapides.", false, true);
        MagmaEvent();
        DisableCurrentEventAfterNumberOfWavesPassed(false);
    }
    private IEnumerator PlayMagmaSound()
    {
        MagmaSound.enabled = true;
        yield return new WaitForSeconds(MagmaSound.clip.length);
        MagmaSound.enabled = false;
    }


    #endregion



    #endregion













    public void DisableCurrentEventAfterNumberOfWavesPassed(bool shouldReduceNbWavesWhenCalled)
    {
        if (currentEvent.nbOfWaves > 0 && shouldReduceNbWavesWhenCalled)
            currentEvent.nbOfWaves--;
        

        if (currentEvent.nbOfWaves == 0)
        {
            RandomEventSystem.instance.isCurrentlyPlayingAnEvent = false;
            currentEvent.nbOfWaves = startNbOfWaves;

            //Msg.print(Msg.ErrorType.Info, "L'event en cours a cessé.");

            switch (currentEvent.eventName)
            {
                case "Blizzard":
                    RevertBlizzard();
                    return;

                case "Magma":
                    RevertMagma();
                    return;

                default:
                    //Msg.print(Msg.ErrorType.Warning, "L'event \"" + currentEvent.eventName + "\" n'a pas de fonction associée.");
                    return;

            }
        }
    }

}
