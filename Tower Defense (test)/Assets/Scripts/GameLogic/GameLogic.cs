using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public static GameLogic instance;


    [Space(10)]

    [Header("Game Over Properties :")]
    public GameObject gameOverUI;
    public Animator animGameOverUi;

    public float delayBeforeGameOverAnim;
    [HideInInspector] public bool isGameOver;

    [Space(5)]

    [Header("Pause options :")]

    public Animator animPauseUi;
    [HideInInspector] public bool isGamePaused;
    public GameObject pauseMenu;
    public KeyCode touchePause;
    public GameObject[] UisToDisable;

    [Space(5)]

    [Header("Pour le tuto :")]
    
    public GameObject tutoVignette;


    [Space(10)]

    [Header("Win Screen Properties :")]
    public GameObject winUI;
    public Animator animWinUI;

    public float delayBeforeWinAnim;
    [HideInInspector] public bool isGameWon;

    private void Awake()
    {
        if (instance != null)
        {
            print("More than one GameLogic in scene !");
            return;
        }

        instance = this;
    }


    private void Start()
    {
        isGameOver = false;
        isGamePaused = false;
        isGameWon = false;
        pauseMenu.SetActive(isGamePaused);
        gameOverUI.SetActive(isGameOver);
        winUI.SetActive(isGameWon);

        if(tutoVignette != null)
        {
            StartCoroutine("WaitBeforeStartingTuto");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(touchePause))
        {
            TogglePauseGame();
        }
    }
    


    public void Win()
    {
        isGameWon = true;
        //Debug.Log("Level won !");
        PlayerPrefs.SetInt("levelReached", SceneManager.GetActiveScene().buildIndex+1);
        StartCoroutine(ShowWinAfterSeconds(delayBeforeWinAnim));
    }


    public void LoadNextLevel(int sceneIndex)
    {
        SceneFader.instance.FadeToScene(sceneIndex);
    }


    private IEnumerator ShowWinAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        winUI.SetActive(true);

        for (int i = 0; i < UisToDisable.Length; i++)
        {
            UisToDisable[i].SetActive(false);
        }

    }









    public void StartGameOver()
    {
        isGameOver = true;
        StartCoroutine(ShowGameOverAfterSeconds(delayBeforeGameOverAnim));
    }

    private IEnumerator ShowGameOverAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverUI.SetActive(true);

        for (int i = 0; i < UisToDisable.Length; i++)
        {
            UisToDisable[i].SetActive(false);
        }
    }







    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        for (int i = 0; i < UisToDisable.Length; i++)
        {
            UisToDisable[i].SetActive(!UisToDisable[i].activeSelf);
        }
        TogglePauseTime();
    }
    
    private void TogglePauseTime()
    {
        if (isGamePaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        
    }





    public void GoToMainMenu(Animator anim)
    {

        if (Time.timeScale == 0f)
        {
            isGamePaused = !isGamePaused;
            TogglePauseTime();
        }
        
        SceneFader.instance.FadeToScene(0);
    }






    public void RestartLevel(Animator anim)
    {
        if (Time.timeScale == 0f)
        {
            isGamePaused = !isGamePaused;
            TogglePauseTime();
        }

        isGameOver = false;

        PlayerStats.instance.Setup_Stats();
        SceneFader.instance.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }




    public void PauseGameForTuto(bool isPaused)
    {
        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            tutoVignette.SetActive(false);
        }
    }

    private IEnumerator WaitBeforeStartingTuto()
    {
        tutoVignette.SetActive(false);
        yield return new WaitForSeconds(1f);
        tutoVignette.SetActive(true);
        PauseGameForTuto(true);
    }


}
