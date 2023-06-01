using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassDoor : MonoBehaviour
{
    public bool CanBeOpened;
    public bool Triggered;
    public Text NotificationText;
    public GameObject HotkeyF;

    private InventoryManagement inventory;

    private void Start()
    {
        CanBeOpened = false;
        Triggered = false;
        inventory = FindObjectOfType<InventoryManagement>();
    }

    private void Update()
    {
        if (Triggered)
            CheckPassKey();
    }

    public void CheckPassKey()
    {
        if (!CanBeOpened)
        {
            NotificationText.text = "Требуется карта работника";
            NotificationText.gameObject.SetActive(true);
        }
        else
        {
            NotificationText.text = "Открыть дверь";
            NotificationText.gameObject.SetActive(true);
            HotkeyF.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
                OpenDoor();
        }
    }

    private void OpenDoor()
    {
        inventory.RemoveFirst();
        Destroy(gameObject);
    }

    public void Stop()
    {
        NotificationText.gameObject.SetActive(false);
        HotkeyF.SetActive(false);
    }
}
