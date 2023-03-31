using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public void UnpauseGame()
    {
        PauseMenu.SetActive(false);
        gameObject.SetActive(true);
    }
}
