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
    public bool IsFreezed;

    public GameObject Hint, KeyCard, GreenKey;

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
    }

    private void InitializeDictionary()
    {
        typeToObject.Add(ItemType.Hint, Hint);
        typeToObject.Add(ItemType.KeyCard, KeyCard);
        typeToObject.Add(ItemType.GreenKey, GreenKey);
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
        else if (type == ItemType.KeyCard)
        {
            var item = typeToObject[type];
            inventoryItems.Add(item);
        }
        else if (type == ItemType.GreenKey)
        {
            inventoryItems.Insert(0, typeToObject[type]);
        }
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
        else if (item.GetComponent<KeyCard>() != null && Elevator == null)
        {
            Add(ItemType.GreenKey, null);
            FindObjectOfType<KitchenDoor>().CanBeOpened = true;
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
            inventoryItems.RemoveAt(0);
        if (currentItemIndex > 0)
            currentItemIndex--;
        else
            currentItemIndex = 0;

        if (inventoryItems.Count == 0)
        {
            Destroy(shownItem);
            shownItem = null;
        }
    }

    private void Update()
    {
        if (!IsFreezed)
            ShowInventoryHotKey();
    }

    private void ShowInventoryHotKey()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isOpened)
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
    GreenKey
}

public enum HintSprite
{
    VaultHint
}