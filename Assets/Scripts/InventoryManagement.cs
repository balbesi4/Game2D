using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{
    public Text InventoryText;
    public GameObject InventoryPanel;
    public GameObject Game;

    private List<GameObject> inventory;
    private bool isOpened;

    private void Awake()
    {
        inventory = new List<GameObject>();
        isOpened = false;
    }

    public void Add(GameObject item)
    {
        inventory.Add(item);
    }

    public void RemoveFirst()
    {
        inventory.RemoveAt(0);
    }

    private void Update()
    {
        if (inventory.Count > 0)
        {
            ShowInventoryHotKey();
        }
    }

    private void ShowInventoryHotKey()
    {
        InventoryText.color = Color.Lerp(Color.grey, Color.white, 0.5f);
        InventoryText.text = "V Инвентарь";
        InventoryText.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.V) && !isOpened)
            OpenInventory();
        else if ((Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Escape)) && isOpened)
            CloseInventory();
    }

    private void OpenInventory()
    {
        Game.SetActive(false);
        InventoryPanel.SetActive(true);
        isOpened = true;
    }

    private void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        Game.SetActive(true);
        isOpened = false;
    }
}
