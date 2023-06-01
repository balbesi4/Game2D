using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject StartButton;
    public GameObject RestartButton;
    public GameObject ContinueButton;

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("Scene to load", 0) == 0)
            SceneManager.LoadScene(1);
        else
        {
            StartButton.SetActive(false);
            RestartButton.SetActive(true);
            ContinueButton.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Scene to load"));
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("Scene to load", 1);
        ContinueGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public enum Scene
{
    MainMenu,
    TutorialBunker,
    GovernmentBunker,
    Laboratory,
    FinalScene
}
