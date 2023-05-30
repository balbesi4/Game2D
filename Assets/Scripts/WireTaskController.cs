using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTaskController : MonoBehaviour
{
    public GameObject Game;
    public GameObject WireTaskPanel;

    public int DetailCount { get; set; }

    public void OpenWireTask()
    {
        WireTaskPanel.SetActive(true);
        FindObjectOfType<WireTask>().DetailsCount = DetailCount;
        Game.SetActive(false);
    }
}
