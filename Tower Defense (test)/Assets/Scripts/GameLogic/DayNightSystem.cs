using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightSystem : MonoBehaviour {
    
    public Vector3 sunSpeed;
    public Transform aiguille;
    public Transform dayAndNightLights;

    public bool isDay;
    public static DayNightSystem instance;

    public GameObject DayLight;
    public GameObject NightLight;

    [Space(20)]



    public float secondsInFullDay = 300f;
    public float dayTime = 0.23f;
    public float nightTime = 0.78f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;


    private bool stopday = false;
    private bool stopnight = false;


    [Space(20)]



    public Text dayOrNightText;
    public Color dayTextColor;
    public Color nightTextColor;


    private void Awake()
    {
        if (instance != null)
        {
            Msg.instance.Print(Msg.ErrorType.Error, 25, "More than one DayNightSystem in scene !", true, false);
            return;
        }

        instance = this;
    }





    void Update()
    {
        UpdateSunRot();

        currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }

        if ( isDay == true)
        {
            NightLight.SetActive(false);
            DayLight.SetActive(true);
        }
        else
        {
            DayLight.SetActive(false);
            NightLight.SetActive(true);
        }
    }








    void UpdateSunRot()
    {
        dayAndNightLights.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        aiguille.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 0, 0);
        
        if (currentTimeOfDay >= dayTime && currentTimeOfDay <= nightTime)
        {
            if (!stopday)
            {
                isDay = true;
                UnitManager.instance.EnableTurretsAtDay();
                Msg.instance.Print(Msg.ErrorType.Info, 25, "Jour.", true, true);


                dayOrNightText.text = "Jour";
                dayOrNightText.color = dayTextColor;
                dayAndNightLights.Find("Moon Light").gameObject.SetActive(false);
                dayAndNightLights.Find("Day Light").gameObject.SetActive(true);

                stopday = true;
                stopnight = false;
            }
        }
        else
        {
            if (!stopnight)
            {
                isDay = false;
                UnitManager.instance.DisableTurretsAtNight();
                Msg.instance.Print(Msg.ErrorType.Info, 17, "Nuit. Placez des spots près de vos tourelles pour leur permettre de repérer les ennemis.", true, true);


                dayOrNightText.text = "Nuit";
                dayOrNightText.color = nightTextColor;
                dayAndNightLights.Find("Moon Light").gameObject.SetActive(true);
                dayAndNightLights.Find("Day Light").gameObject.SetActive(false);

                stopnight = true;
                stopday = false;
            }
        }
    }
}
