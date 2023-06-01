using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TelephonDialog : MonoBehaviour
{
    public Text MainText;

    private int phrazeIndex = 0;

    private string[] phrazes = new[]
    {
        "Здесь будет финальный диалог",
        "Здесь тоже..."
    };

    private void Update()
    {
        MainText.text = phrazes[phrazeIndex];

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (phrazeIndex < phrazes.Length - 1)
                phrazeIndex++;
            else
            {
                PlayerPrefs.SetInt("Scene to load", 0);
                SceneManager.LoadScene((int)Scene.MainMenu);
            }
        }
    }
}
