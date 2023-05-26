﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VR;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public ElevatorManagement Elevator;
    public Text NotificationText, InventoryText;
    public Image HotkeyF, HotkeyV;
    public GameObject Message;
    public bool IsFreezed;

    private InventoryManagement playerInventory;
    private Rigidbody2D rb;
    private GameObject currentCollision;
    private Queue<GameObject> messageObjects;
    private GunController gunContoller;
    private GunsUI gunsUI;

    private float playerSpeed = 4f;
    private bool canBeTaken, canCutSceneBeStarted, canOpenElevator, canOpenComputer, canSwitch;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInventory = mainCamera.GetComponent<InventoryManagement>();
        canBeTaken = false;
        canOpenElevator = false;
        canCutSceneBeStarted = false;
        IsFreezed = false;
        canOpenComputer = false;
        canSwitch = false;
        messageObjects = new Queue<GameObject>();
        gunContoller = GetComponent<GunController>();
        gunsUI = FindObjectOfType<GunsUI>();
    }

    public void Update()
    {
        if (!IsFreezed)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (canBeTaken)
                    TakeItem();
                else if (canCutSceneBeStarted)
                    FindObjectOfType<GirlCutScene>().StartGirlCutScene();
                else if (canOpenComputer)
                    FindObjectOfType<ComputerController>().OpenComputer();
                else if (canSwitch)
                    FindObjectOfType<Switcher>().ChangeSwitcher();
            }
            if (canOpenElevator)
                Elevator.CheckKeyCard();
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (!gunsUI.IsChoosing)
                {
                    gunContoller.SetCurrentGun(0);
                    gunsUI.ChooseGun(0);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!gunsUI.IsChoosing)
                {
                    gunContoller.SetCurrentGun(1);
                    gunsUI.ChooseGun(1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!gunsUI.IsChoosing)
                {
                    gunContoller.SetCurrentGun(2);
                    gunsUI.ChooseGun(2);
                }
            }
        }
        else
        {
            rb.velocity = new Vector3(0, 0);
        }
    }

    private void Move()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0 || vertical == 0)
            rb.velocity = new Vector3(horizontal, vertical, 0) * playerSpeed;
        else
            rb.velocity = new Vector3
                (horizontal / (float)Math.Sqrt(2),
                vertical / (float)Math.Sqrt(2))
                * playerSpeed;
    }

    private void TakeItem()
    {
        if (currentCollision.GetComponent<GunToGrab>() != null)
        {
            gunContoller.Add(currentCollision.GetComponent<GunToGrab>().GunType);
            Destroy(currentCollision);
            return;
        }
        else if (currentCollision.GetComponent<Medkit>() != null)
        {
            var healPoints = currentCollision.GetComponent<Medkit>().HealPoints;
            GetComponent<PlayerHealthManagement>().Heal(healPoints);
            Destroy(currentCollision);
            return;
        }
        else if (currentCollision.GetComponent<AmmoBox>() != null)
        {
            var ammoBoxInfo = currentCollision.GetComponent<AmmoBox>();
            if (gunContoller.Contains(ammoBoxInfo.ThisGun))
            {
                GetComponent<BulletCount>().Add(ammoBoxInfo.BulletCount, ammoBoxInfo.ThisGun);
                Destroy(currentCollision);
            }
            else
            {
                StartCoroutine(FindObjectOfType<GunsUI>().ShowMessage("Нет нужного оружия"));
                if (HotkeyF.gameObject.activeSelf)
                    HotkeyF.gameObject.SetActive(false);
            }
            return;
        }
        else
            StartCoroutine(ShowTakingItem());

        playerInventory.Add(currentCollision);
        Destroy(currentCollision);
    }

    private IEnumerator ShowTakingItem()
    {
        var oldColor = InventoryText.color;
        InventoryText.fontSize += 8;
        InventoryText.color = Color.white;
        HotkeyV.rectTransform.sizeDelta += new Vector2(8, 8);

        yield return new WaitForSeconds(0.5f);
        InventoryText.fontSize -= 8;
        InventoryText.color = oldColor;
        HotkeyV.rectTransform.sizeDelta -= new Vector2(8, 8);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            collision.gameObject.GetComponentInParent<DoorManagement>().StartRoomAction();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Drop"))
        {
            ShowMessage(collision.gameObject, true);
            NotificationText.text = "Подобрать";
            NotificationText.color = Color.Lerp(Color.white, Color.gray, 0.5f);
            NotificationText.gameObject.SetActive(true);
            HotkeyF.gameObject.SetActive(true);
            currentCollision = collision.gameObject;
            canBeTaken = true;
        }
        else if (collision.gameObject.CompareTag("Cut scene"))
        {
            NotificationText.text = "Поговорить";
            NotificationText.color = Color.Lerp(Color.white, Color.gray, 0.5f);
            NotificationText.gameObject.SetActive(true);
            HotkeyF.gameObject.SetActive(true);
            canCutSceneBeStarted = true;
        }
        else if (collision.gameObject.CompareTag("Computer"))
        {
            NotificationText.text = "Воспользоваться";
            NotificationText.color = Color.Lerp(Color.white, Color.gray, 0.5f);
            NotificationText.gameObject.SetActive(true);
            HotkeyF.gameObject.SetActive(true);
            canOpenComputer = true;
        }
        else if (collision.gameObject.CompareTag("Switcher"))
        {
            NotificationText.text = "Выкл/Вкл компьютер";
            NotificationText.color = Color.Lerp(Color.white, Color.gray, 0.5f);
            NotificationText.gameObject.SetActive(true);
            HotkeyF.gameObject.SetActive(true);
            canSwitch = true;
        }
        else if (collision.gameObject.CompareTag("Medkit"))
        {
            ShowMessage(collision.gameObject, true);
            NotificationText.text = "Использовать";
            NotificationText.color = Color.Lerp(Color.white, Color.gray, 0.5f);
            NotificationText.gameObject.SetActive(true);
            HotkeyF.gameObject.SetActive(true);
            currentCollision = collision.gameObject;
            canBeTaken = true;
        }
        else if (collision.gameObject.CompareTag("Boost"))
        {
            StartCoroutine(Boost());
        }
        else if (collision.gameObject.CompareTag("Elevator"))
        {
            canOpenElevator = true;
        }
        else if (collision.gameObject.CompareTag("Kitchen door"))
        {
            FindObjectOfType<KitchenDoor>().Triggered = true;
        }
        else if (collision.gameObject.CompareTag("Card door"))
        {
            FindObjectOfType<CardDoor>().Triggered = true;
        }
    }

    private IEnumerator Boost()
    {
        playerSpeed = 6f;
        yield return new WaitForSeconds(1f);
        playerSpeed = 4f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Drop"))
        {
            ShowMessage(collision.gameObject, false);
            NotificationText.gameObject.SetActive(false);
            HotkeyF.gameObject.SetActive(false);
            currentCollision = null;
            canBeTaken = false;
        }
        else if (collision.gameObject.CompareTag("Medkit"))
        {
            ShowMessage(collision.gameObject, false);
            NotificationText.gameObject.SetActive(false);
            HotkeyF.gameObject.SetActive(false);
            currentCollision = null;
            canBeTaken = false;
        }
        else if (collision.gameObject.CompareTag("Cut scene"))
        {
            NotificationText.gameObject.SetActive(false);
            HotkeyF.gameObject.SetActive(false);
            canCutSceneBeStarted = false;
        }
        else if (collision.gameObject.CompareTag("Computer"))
        {
            NotificationText.gameObject.SetActive(false);
            HotkeyF.gameObject.SetActive(false);
            canOpenComputer = false;
        }
        else if (collision.gameObject.CompareTag("Switcher"))
        {
            NotificationText.gameObject.SetActive(false);
            HotkeyF.gameObject.SetActive(false);
            canSwitch = false;
        }
        else if (collision.gameObject.CompareTag("Elevator"))
        {
            Elevator.Stop();
            canOpenElevator = false;
        }
        else if (collision.gameObject.CompareTag("Kitchen door"))
        {
            FindObjectOfType<KitchenDoor>().Triggered = false;
            FindObjectOfType<KitchenDoor>().Stop();
        }
        else if (collision.gameObject.CompareTag("Card door"))
        {
            FindObjectOfType<CardDoor>().Triggered = false;
            FindObjectOfType<CardDoor>().Stop();
        }
    }

    private void ShowMessage(GameObject collision, bool setActive)
    {
        if (setActive)
        {
            var collisionPos = collision.transform.position;
            var spawnPos = new Vector3(collisionPos.x + 0.7f, collisionPos.y + 0.3f, collisionPos.z);
            var messageText = Message.GetComponentInChildren<Text>();
            if (collision.TryGetComponent(out Hint hint))
                messageText.text = hint.MessageUI;
            else if (collision.TryGetComponent(out KeyCard keycard))
                messageText.text = keycard.MessageUI;
            else if (collision.TryGetComponent(out GunToGrab gun))
                messageText.text = gun.MessageUI;
            else if (collision.TryGetComponent(out Medkit medkit))
                messageText.text = medkit.MessageUI;
            else if (collision.TryGetComponent(out AmmoBox ammoBox))
                messageText.text = ammoBox.MessageUI;

            messageText.color = Color.white;
            
            messageObjects.Enqueue(Instantiate(Message, spawnPos, Quaternion.identity));
        }
        else
        {
            Destroy(messageObjects.Dequeue());
        }
    }
}
