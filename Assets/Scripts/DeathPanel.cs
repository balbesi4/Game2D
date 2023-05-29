using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    public void LoadMenu()
    {
        PlayerPrefs.SetInt("Scene to load", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
