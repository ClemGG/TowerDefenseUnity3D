using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {
    
    public GameObject levelSelectionPanel;

    [Space(10)]

    private GameObject menuTurret;
    public GameObject[] listOfTurrets;


    // Use this for initialization
    void Start () {
        menuTurret = listOfTurrets[Random.Range(0, listOfTurrets.Length)];
        menuTurret.SetActive(!menuTurret.activeSelf);
        levelSelectionPanel.SetActive(false);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("levelReached");
        SceneFader.instance.FadeToScene(1);
    }

    public void StartSelectedLevel(int selectedlevelIndex)
    {
        SceneFader.instance.FadeToScene(selectedlevelIndex);
    }

    public void ToggleLevelSelectionPanel()
    {
        levelSelectionPanel.SetActive(!levelSelectionPanel.activeSelf);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
