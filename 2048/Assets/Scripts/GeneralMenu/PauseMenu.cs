using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject restartUI;
    public GameObject gameBoard;

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        gameBoard.GetComponent<TileBoard>().enabled = true;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        gameBoard.GetComponent<TileBoard>().enabled = false;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        gameBoard.GetComponent<TileBoard>().enabled = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void ResumeRestartGO()
    {
        restartUI.SetActive(false);
        gameBoard.GetComponent<TileBoard>().enabled = true;
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void PauseRestartGO()
    {
        restartUI.SetActive(true);
        gameBoard.GetComponent<TileBoard>().enabled = false;
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
