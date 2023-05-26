using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public GameObject Level;
    public GameObject TaskPanel;

    public void OpenComputer()
    {
        TaskPanel.SetActive(true);
        Level.SetActive(false);
    }
}
