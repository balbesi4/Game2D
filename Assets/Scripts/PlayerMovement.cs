using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public List<DoorManagement> Rooms;
    public Text NotificationText;

    private InventoryManagement playerInventory;
    private Rigidbody2D rb;
    private GameObject currentCollision;

    private float playerSpeed = 4f;
    private int roomCounter;
    private bool canBeTaken;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInventory = mainCamera.GetComponent<InventoryManagement>();
        roomCounter = 0;
        canBeTaken = false;
    }

    public void Update()
    {
        Move();
        if (canBeTaken && Input.GetKeyDown(KeyCode.F))
            TakeItem();
    }

    private void Move()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector3(horizontal, vertical, 0) * playerSpeed;
    }

    private void TakeItem()
    {
        playerInventory.Add(currentCollision);
        Destroy(currentCollision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            Destroy(collision.gameObject);
            Rooms[roomCounter].StartRoomAction();
            roomCounter++;
        }

        else if (collision.gameObject.CompareTag("Drop"))
        {
            NotificationText.text = "F подобрать";
            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);
            currentCollision = collision.gameObject;
            canBeTaken = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Drop"))
        {
            NotificationText.gameObject.SetActive(false);
            currentCollision = null;
            canBeTaken = false;
        }
    }
}

