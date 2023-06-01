using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagement : MonoBehaviour
{
    public Text InventoryText;
    public GameObject InventoryPanel;
    public GameObject EmptyInventoryText;
    public GameObject Game;
    public GameObject RightArrowButton;
    public GameObject LeftArrowButton;
    public List<Sprite> HintSprites;
    public ElevatorManagement Elevator;
    public bool IsFreezed, IsTakingItem;

    public GameObject Hint, KeyCard, GreenKey, EmployeeCard;
    public GameObject WireDetail1, WireDetail2, WireDetail3;
    public GameObject PassCard, ChilloutKey;

    private List<GameObject> inventoryItems;
    private Dictionary<ItemType, GameObject> typeToObject;
    private GameObject shownItem;
    private int currentItemIndex;
    private bool isOpened;
    private bool areButtonsEnabled;

    private void Awake()
    {
        inventoryItems = new List<GameObject>();
        isOpened = false;
        typeToObject = new Dictionary<ItemType, GameObject>();
        InitializeDictionary();
        InventoryText.color = Color.Lerp(Color.grey, Color.white, 0.5f);
        InventoryText.text = "Инвентарь";
        IsFreezed = false;
        IsTakingItem = false;
    }

    private void InitializeDictionary()
    {
        typeToObject.Add(ItemType.Hint, Hint);
        typeToObject.Add(ItemType.KeyCard, KeyCard);
        typeToObject.Add(ItemType.GreenKey, GreenKey);
        typeToObject.Add(ItemType.EmployeeCard, EmployeeCard);
        typeToObject.Add(ItemType.WireDetail1, WireDetail1);
        typeToObject.Add(ItemType.WireDetail2, WireDetail2);
        typeToObject.Add(ItemType.WireDetail3, WireDetail3);
        typeToObject.Add(ItemType.ChilloutRoomKey, ChilloutKey);
        typeToObject.Add(ItemType.PassCard, PassCard);
    }

    public void Add(ItemType type, HintSprite? sprite)
    {
        if (type == ItemType.Hint)
        {
            var item = typeToObject[type];
            var sprites = item.GetComponentsInChildren<Image>();
            sprites[sprites.Length - 1].sprite = HintSprites[(int)sprite];
            inventoryItems.Add(item);
        }
        else if (type == ItemType.EmployeeCard)
            inventoryItems.Insert(0, typeToObject[type]);
        else
            inventoryItems.Add(typeToObject[type]);
    }

    public void Add(GameObject item)
    {
        if (item.GetComponent<Hint>() != null)
            Add(ItemType.Hint, item.GetComponent<Hint>().ThisHintSprite);
        else if (item.GetComponent<KeyCard>() != null && Elevator != null)
        {
            Add(ItemType.KeyCard, null);
            Elevator.isKeyGrabbed = true;
        }
        else if (item.GetComponent<KeyCard>() != null && item.GetComponent<KeyCard>().ItemType == ItemType.GreenKey && Elevator == null)
        {
            Add(ItemType.GreenKey, null);
            FindObjectOfType<KitchenDoor>().CanBeOpened = true;
        }
        else if (item.GetComponent<KeyCard>() != null && item.GetComponent<KeyCard>().ItemType == ItemType.EmployeeCard && Elevator == null)
        {
            Add(ItemType.EmployeeCard, null);
            FindObjectOfType<CardDoor>().CanBeOpened = true;
        }
        else if (item.GetComponent<KeyCard>() != null && item.GetComponent<KeyCard>().ItemType == ItemType.PassCard && Elevator == null)
        {
            Add(ItemType.PassCard, null);
            FindObjectOfType<PassDoor>().CanBeOpened = true;
        }
        else if (item.GetComponent<KeyCard>() != null && item.GetComponent<KeyCard>().ItemType == ItemType.ChilloutRoomKey && Elevator == null)
        {
            Add(ItemType.ChilloutRoomKey, null);
            FindObjectOfType<ChilloutDoor>().CanBeOpened = true;
        }
        else if (item.GetComponent<WireTaskDetail>() != null)
        {
            var wireDetail = item.GetComponent<WireTaskDetail>();
            if (wireDetail.Index == 1)
                Add(ItemType.WireDetail1, null);
            else if (wireDetail.Index == 2)
                Add(ItemType.WireDetail2, null);
            else
                Add(ItemType.WireDetail3, null);

            FindObjectOfType<WireTaskController>().DetailCount++;
        }
    }

    public void Clear()
    {
        inventoryItems.Clear();
        currentItemIndex = 0;
        if (shownItem != null) 
            Destroy(shownItem);
        shownItem = null;
        areButtonsEnabled = false;
        SetButtonsIntensity();
    }

    public void RemoveFirst()
    {
        if (inventoryItems.Count > 0)
        {
            inventoryItems.RemoveAt(0);
            if (currentItemIndex > 0)
                currentItemIndex--;
            else if (currentItemIndex == 0 && inventoryItems.Count > 0)
            {
                Destroy(shownItem);
                shownItem = Instantiate(inventoryItems[currentItemIndex], InventoryPanel.transform);
            }
            else if (inventoryItems.Count == 0)
            {
                Destroy(shownItem);
                shownItem = null;
            }
        }
    }

    private void Update()
    {
        if (!IsFreezed)
            ShowInventoryHotKey();
    }

    private void ShowInventoryHotKey()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isOpened && !IsTakingItem)
            OpenInventory();
        else if ((Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Escape)) && isOpened)
            CloseInventory();
    }

    private void OpenInventory()
    {
        Game.SetActive(false);
        InventoryPanel.SetActive(true);
        ShowInventoryItems();
        SetButtonsIntensity();
        isOpened = true;
    }

    private void ShowInventoryItems()
    {
        if (inventoryItems.Count == 0)
        {
            if (shownItem != null && shownItem.GetComponent<Text>() == null)
            {
                Destroy(shownItem);
                shownItem = Instantiate(EmptyInventoryText, InventoryPanel.transform);
            }
            else if (shownItem == null)
                shownItem = Instantiate(EmptyInventoryText, InventoryPanel.transform);
        }
        else
            ManageInventory();
    }

    private void ManageInventory()
    {
        if (inventoryItems.Count == 1)
        {
            if (shownItem != null && shownItem.GetComponent<Text>() != null)
            {
                Destroy(shownItem);
                shownItem = null;
            }
            if (shownItem == null)
                shownItem = Instantiate(inventoryItems[currentItemIndex], InventoryPanel.transform);
            areButtonsEnabled = false;
        }
        else
        {
            if (shownItem != null && shownItem.GetComponent<Text>() != null)
            {
                Destroy(shownItem);
                shownItem = null;
            }
            if (shownItem == null)
                shownItem = Instantiate(inventoryItems[currentItemIndex], InventoryPanel.transform);
            areButtonsEnabled = true;
        }
    }

    private void SetButtonsIntensity()
    {
        var leftButton = currentItemIndex != 0;
        var rightButton = currentItemIndex != inventoryItems.Count - 1;

        var brightColor = new Color(1, 1, 1, 1);
        var darkColor = new Color(1, 1, 1, 0.4f);
        var leftImage = LeftArrowButton.GetComponent<Image>();
        var rightImage = RightArrowButton.GetComponent<Image>();

        if (!areButtonsEnabled)
        {
            leftImage.color = darkColor;
            rightImage.color = darkColor;
        }
        else
        {
            if (leftButton)
                leftImage.color = brightColor;
            else
                leftImage.color = darkColor;

            if (rightButton)
                rightImage.color = brightColor;
            else
                rightImage.color = darkColor;
        }
    }

    public void SwitchItemToRight(int direction)
    {
        if (currentItemIndex + direction > inventoryItems.Count - 1 || !areButtonsEnabled)
            return;
        Destroy(shownItem);
        currentItemIndex += direction;
        shownItem = Instantiate(inventoryItems[currentItemIndex], InventoryPanel.transform);
        SetButtonsIntensity();
    }
    
    public void SwitchItemToLeft(int direction)
    {
        if (currentItemIndex + direction < 0 || !areButtonsEnabled)
            return;
        Destroy(shownItem);
        currentItemIndex += direction;
        shownItem = Instantiate(inventoryItems[currentItemIndex], InventoryPanel.transform);
        SetButtonsIntensity();
    }

    public void CloseInventory()
    {
        InventoryPanel.SetActive(false);
        Game.SetActive(true);
        isOpened = false;
    }
}

public enum ItemType
{
    Hint,
    KeyCard,
    GreenKey,
    EmployeeCard,
    WireDetail1,
    WireDetail2,
    WireDetail3,
    PassCard,
    ChilloutRoomKey
}

public enum HintSprite
{
    VaultHint
}