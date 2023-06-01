using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManagement : MonoBehaviour
{
    public GameObject PauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            PauseMenu.SetActive(true);
        }
    }

    public void OpenMenu()
    {
        PlayerPrefs.SetInt("Scene to load", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene((int)Scene.MainMenu);
    }

    public void UnpauseGame()
    {
        PauseMenu.SetActive(false);
        gameObject.SetActive(true);
    }
}
