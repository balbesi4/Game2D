using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VaultController : MonoBehaviour
{
    public Text NotificationText;
    public GameObject HotkeyF;
    public Camera MainCamera;
    public GameObject VaultTask;
    public GameObject Game;
    public GameObject Key;
    public InventoryManagement Inventory;
    public bool IsPassed;

    private bool canBeOpened;

    private void Start()
    {
        canBeOpened = false;
        IsPassed = false;
    }

    private void Update()
    {
        if (canBeOpened && Input.GetKeyDown(KeyCode.F))
        {
            MainCamera.gameObject.SetActive(false);
            Game.SetActive(false);
            VaultTask.SetActive(true);
        }
        if (IsPassed)
        {
            var triggerPos = gameObject.transform.position;
            var dropPos = new Vector3(triggerPos.x - 1, triggerPos.y, triggerPos.z);
            Instantiate(Key, dropPos, Quaternion.identity);
            Inventory.RemoveFirst();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealthManagement player))
        {
            NotificationText.color = Color.white;
            NotificationText.text = "Открыть сейф";
            NotificationText.gameObject.SetActive(true);
            HotkeyF.SetActive(true);
            canBeOpened = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealthManagement player))
        {
            NotificationText.gameObject.SetActive(false);
            HotkeyF.SetActive(false);
            canBeOpened = false;
        }
    }
}
