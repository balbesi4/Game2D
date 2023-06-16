using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMashineTask : MonoBehaviour
{
    public GameObject Trigger;
    public GameObject NotificationPanel;
    public Call CallPanel;

    private int[] correctInput;
    private List<int> input;
    private bool attempted;

    private void Start()
    {
        attempted = false;
        input = new();
        correctInput = new[] { 1, 3 };
    }

    public void Confirm()
    {
        if (!attempted)
        {
            CallPanel.On();
            attempted = true;
        }

        if (input.Count != correctInput.Length)
        {
            Clear();
            return;
        }

        for (var i = 0; i < input.Count; i++)
            if (input[i] != correctInput[i])
            {
                Clear();
                return;
            }

        Trigger.GetComponent<TimeMachineController>().IsTaskPassed = true;
        CloseTask();
    }

    public void Click(int value)
    {
        input.Add(value);
    }

    public void Clear()
    {
        input.Clear();
    }

    public void CloseTask()
    {
        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<InventoryManagement>().IsFreezed = false;

        Trigger.SetActive(true);
        NotificationPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
