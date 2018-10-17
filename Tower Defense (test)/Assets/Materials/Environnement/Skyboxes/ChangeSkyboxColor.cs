using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxColor : MonoBehaviour {

    [SerializeField] private Color[] skyboxColors, sunColors;
    [SerializeField] private Light dirLight;
    [SerializeField] private Material skybox;

    [SerializeField] private DayNightSystem SunControl;
    [SerializeField] private float fadeSpeed;

    private int i = 0;
    private float indexSuivant;

	// Use this for initialization
	void Start ()
    {
        skybox.SetColor("_SkyTint", skyboxColors[0]);
        dirLight.color = sunColors[0];
        SunControl = GetComponent<DayNightSystem>();

        indexSuivant = Mathf.Abs(Mathf.Deg2Rad * 180f * ((float)(i + 1) / (float)sunColors.Length)) / Mathf.PI;
    }
	
	// Update is called once per frame
	void Update ()
    {
         Update_SkyBox_Colors();
	}



//A FAIRE : Uniformiser les durées que chaque couleur pourqu'elles soient similaires
    private void Update_SkyBox_Colors()
    {

        float indexActuel = Mathf.Abs(Mathf.Deg2Rad * 180f * (dirLight.transform.rotation.x * ((float)(i + 1) / (float)sunColors.Length))) / Mathf.PI;



        if (i >= sunColors.Length && Maths.FastApproximately(indexActuel, 0f, 0.05f))
        {
            i = 0;
            indexSuivant = Mathf.Abs(Mathf.Deg2Rad * 180f * ((float)(i + 1) / (float)sunColors.Length)) / Mathf.PI;
        }


        //print(i);
        //print("indexActuel = " + indexActuel + " ; indexSuivant = " + indexSuivant + " ; Résultat de l'approximation : " + Maths.FastApproximatelyWithFirstArgumentAsInferiorStrict(indexActuel, indexSuivant, 0.1f));
        //print(indexActuel);
        //print(indexSuivant);

        //print((Mathf.Deg2Rad * 180f * dirLight.transform.localRotation.x ) / Mathf.PI);
        //print((float)(i + 1) / (float)sunColors.Length - 0.1f);

        if (i < sunColors.Length-1)
        {
            dirLight.color = Color.Lerp(dirLight.color,
                                        sunColors[Mathf.RoundToInt(indexSuivant) + i],
                                        Time.deltaTime * fadeSpeed);

            skybox.SetColor("_SkyTint", Color.Lerp(skybox.GetColor("_SkyTint"),
                                        skyboxColors[Mathf.RoundToInt(indexSuivant) + i],
                                        Time.deltaTime * fadeSpeed));
        }

        if (Maths.FastApproximatelyWithFirstArgumentAsInferiorStrict(indexActuel, indexSuivant, 0.1f))
        {
            print("indexActuel = " + indexActuel + " ; indexSuivant = " + indexSuivant);
            i++;
            indexSuivant = Mathf.Abs(Mathf.Deg2Rad * 180f * ((float)(i + 1) / (float)sunColors.Length)) / Mathf.PI;
        }
    }
}
