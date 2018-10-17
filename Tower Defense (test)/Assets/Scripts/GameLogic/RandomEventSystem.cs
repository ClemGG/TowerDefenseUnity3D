
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventSystem : MonoBehaviour {

    public Text currentEventNameText;


    public float minDelayBetweenTwoEvents = 120f;
    private float timer = 1f;
    


    [HideInInspector] public bool isCurrentlyPlayingAnEvent;
    public EventToPlay[] events;

    private EventToPlay eventToPlay;
    




    public static RandomEventSystem instance;

    private void Awake()
    {
        timer = minDelayBetweenTwoEvents;

        if (instance != null)
        {
            Msg.instance.Print(Msg.ErrorType.Error, 25, "More than one RandomEventSystem in scene !", true, false);
            return;
        }

        instance = this;

        eventToPlay = new EventToPlay("Aucun événement", 10f, 2, Color.black, 10f);
    }








	// Update is called once per frame
	void Update () {

        #region Calcul du délai entre le nouvel intervalle et le précédent
        if (!isCurrentlyPlayingAnEvent)
        {

            if (timer <= 0f)
            {
                eventToPlay = SelectRandomEvent();
                isCurrentlyPlayingAnEvent = true;
                EventScript.instance.PlayCorrespondingEffect(eventToPlay);
                Msg.instance.Print(Msg.ErrorType.Warning, 21, "L'évenement " + eventToPlay.eventName + " sera actif dans " +Mathf.RoundToInt(eventToPlay.delayBeforeEventActivation).ToString()+" secondes.", true, true);
                timer = minDelayBetweenTwoEvents;
            }

            timer -= Time.deltaTime;
        }
        #endregion

        Update_Countdown_UI();

    }




    private void Update_Countdown_UI()
    {

        if (!isCurrentlyPlayingAnEvent)
        {

            currentEventNameText.text = "Aucun événement";
            currentEventNameText.color = Color.black;

        }
        else if(isCurrentlyPlayingAnEvent)
        {
            currentEventNameText.text = eventToPlay.eventName;
            currentEventNameText.color = eventToPlay.textColor;
        }

    }











    private EventToPlay SelectRandomEvent()
    {
        EventToPlay e;
        e = eventToPlay;
        for (int i = 0; i < events.Length; i++)
        {
            if(events[i].eventName != eventToPlay.eventName)
            {
                e = events[i];
                break;
            }
        }



        return e;
    }








}













[System.Serializable]
public class EventToPlay
{
    public string eventName;
    [Range(0,100f)]public float spotChancePercentage;
    public int nbOfWaves;
    public Color textColor;
    public float delayBeforeEventActivation;


    public EventToPlay(string name, float chance, int nbwaves, Color col, float delay)
    {
        eventName = name;
        spotChancePercentage = chance;
        nbOfWaves = nbwaves;
        textColor = col;
        delayBeforeEventActivation = delay;
    }
}
