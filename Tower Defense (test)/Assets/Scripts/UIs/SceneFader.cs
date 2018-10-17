using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    public Image fadeImg;

    [Tooltip("Plus la valeur est haute, plus le fondu sera rapide, et inversement pus la valeur est basse.")]
    public float fadeInSpeed = 1f;
    [Tooltip("Plus la valeur est haute, plus le fondu sera rapide, et inversement pus la valeur est basse.")]
    public float fadeOutSpeed = 1f;

    public Color fadeColor = Color.black;
    public AnimationCurve fadeCurve;


    public static SceneFader instance;




    private void Awake()
    {
        if (instance != null)
        {
            print("More than one SceneFader in scene !");
            return;
        }

        instance = this;
    }





    private void Start()
    {
        StartCoroutine(FadeIn());
    }









    /// <summary>
    /// Permet de réaliser un fondu entre les scènes
    /// </summary>
    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }





    /// <summary>
    /// Diminue l'alpha du fondu pour faire apparaître la scène
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * fadeInSpeed;
            float a = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(fadeColor.r,fadeColor.g,fadeColor.b, a);
            yield return 0;
        }
    }



    /// <summary>
    /// Augmente l'alpha du fondu pour faire disparaître la scène
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut(int sceneIndex)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * fadeOutSpeed;
            float a = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, a);
            yield return 0;
        }

        SceneManager.LoadScene(sceneIndex);
    }
}
