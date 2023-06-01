using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Explanation : MonoBehaviour
{
    public Text MainText;

    private int phrazeIndex = 0;

    private string[] phrazes = new[]
    {
        "«десь будет объ€снение того, как, что и почему",
        "«десь тоже..."
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
                PlayerPrefs.SetInt("Scene to load", (int)Scene.TutorialBunker);
                SceneManager.LoadScene((int)Scene.TutorialBunker);
            }
        }
    }
}
