using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public ElevatorManagement Elevator;
    public Text NotificationText;
    public GameObject Message;

    private InventoryManagement playerInventory;
    private Rigidbody2D rb;
    private GameObject currentCollision;
    private Queue<GameObject> messageObjects;

    private float playerSpeed = 4f;
    private bool canBeTaken;
    private bool canOpenElevator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInventory = mainCamera.GetComponent<InventoryManagement>();
        canBeTaken = false;
        canOpenElevator = false;
        messageObjects = new Queue<GameObject>();
    }

    public void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.F) && canBeTaken)
            TakeItem();
        if (canOpenElevator)
            Elevator.CheckKeyCard();
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
            collision.gameObject.GetComponentInParent<DoorManagement>().StartRoomAction();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Drop"))
        {
            ShowMessage(collision.gameObject, true);
            NotificationText.text = "F подобрать";
            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);
            currentCollision = collision.gameObject;
            canBeTaken = true;
        }
        else if (collision.gameObject.CompareTag("Elevator"))
        {
            canOpenElevator = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Drop"))
        {
            ShowMessage(collision.gameObject, false);
            NotificationText.gameObject.SetActive(false);
            currentCollision = null;
            canBeTaken = false;
        }
        else if (collision.gameObject.CompareTag("Elevator"))
        {
            Elevator.Stop();
            canOpenElevator = false;
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
            messageText.color = Color.white;
            
            messageObjects.Enqueue(Instantiate(Message, spawnPos, Quaternion.identity));
        }
        else
        {
            Destroy(messageObjects.Dequeue());
        }
    }
}

