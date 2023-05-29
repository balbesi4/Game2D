﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElevatorManagement : MonoBehaviour
{
    public GameObject Indicator;
    public GameObject ElevatorMainDoor;
    public GameObject ElevatorDoors;
    public InventoryManagement Inventory;
    public Text NotificationText;
    public GameObject HotkeyF;
    public bool isKeyGrabbed;

    private bool isElevatorOpened;

    private void Awake()
    {
        isKeyGrabbed = false;
        isElevatorOpened = false;
    }

    public void CheckKeyCard()
    {
        if (isKeyGrabbed)
        {
            HotkeyF.SetActive(true);

            if (!isElevatorOpened)
                NotificationText.text = "Открыть лифт";
            else
                NotificationText.text = "Зайти в лифт";
            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
                OpenElevator();
        }
        else
        {
            NotificationText.text = "Нужна ключ-карта";
            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);
        }
    }

    public void Stop()
    {
        NotificationText.gameObject.SetActive(false);
        HotkeyF.SetActive(false);
    }

    private void OpenElevator()
    {
        if (!isElevatorOpened)
        {
            ElevatorMainDoor.SetActive(false);
            ElevatorDoors.SetActive(true);
            Indicator.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.4f);
            NotificationText.text = "Зайти в лифт";
            isElevatorOpened = true;
            Inventory.Clear();
        }
        else
        {
            PlayerPrefs.SetInt("Scene to load", (int)Scene.GovernmentBunker);
            SceneManager.LoadScene(PlayerPrefs.GetInt("Scene to load"));
        }
    }
}
