using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTaskController : MonoBehaviour
{
    public GameObject Level;
    public GameObject TaskPanel;

    public void OpenTask()
    {
        TaskPanel.SetActive(true);
        Level.SetActive(false);
    }
}
