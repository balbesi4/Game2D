using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public void EnterPortal()
    {
        PlayerPrefs.SetInt("Scene to load", (int)Scene.Laboratory);
        SceneManager.LoadScene((int)Scene.Laboratory);
    }
}
