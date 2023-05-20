using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunsUI : MonoBehaviour
{
    public GameObject GunPanel;
    public GameObject GunUI;
    public Sprite AKSprite;
    public Sprite Hotkey2Sprite;
    public Sprite Hotkey3Sprite;

    private int currentGun;
    private bool isChoosing;
    private Vector2 defaultSize;
    private Vector2 highlightedSize;
    private Dictionary<Gun, Sprite> gunsToSprites;
    private Dictionary<int,  Sprite> numberToHotkey;
    private List<GameObject> guns;
    private List<Image> hotkeys;
    private List<Image> gunImages;

    private void Awake()
    {
        gunsToSprites = new();
        guns = new();
        hotkeys = new();
        gunImages = new();
        numberToHotkey = new();
        currentGun = 0;
        isChoosing = false;
    }

    private void Start()
    {
        gunsToSprites.Add(Gun.AK, AKSprite);
        numberToHotkey.Add(2, Hotkey2Sprite);
        numberToHotkey.Add(3, Hotkey3Sprite);
        var pistol = Instantiate(GunUI, GunPanel.transform);
        guns.Add(pistol);
        var images = pistol.GetComponentsInChildren<Image>();
        var hotkey = images[1];
        var image = images[0];
        hotkeys.Add(hotkey);
        gunImages.Add(image);
        defaultSize = new Vector2(50, 50);
        highlightedSize = defaultSize + new Vector2(15, 15);
        HighlightGun(currentGun);
    }

    public void Add(Gun gun)
    {
        var hotkeySprite = numberToHotkey[(int)gun + 1];
        var gunSprite = gunsToSprites[gun];
        var position = guns.Last().transform.position + new Vector3(0, 80f);
        var newGun = Instantiate(GunUI, position, Quaternion.identity, GunPanel.transform);

        var images = newGun.GetComponentsInChildren<Image>();
        var hotkey = images[1];
        var gunImage = images[0];
        gunImage.sprite = gunSprite;
        hotkey.sprite = hotkeySprite;

        guns.Add(newGun);
        hotkeys.Add(hotkey);
        gunImages.Add(gunImage);
        HighlightGun(currentGun);
    }

    public void ChooseGun(int index)
    {
        if (index >= hotkeys.Count) return;
        if (index != currentGun)
        {
            currentGun = index;
            HighlightGun(currentGun);
        }
        if (!isChoosing)
            StartCoroutine(ShowChoosingGun());
    }

    private IEnumerator ShowChoosingGun()
    {
        isChoosing = true;
        gunImages[currentGun].rectTransform.sizeDelta += new Vector2(5, 5);
        hotkeys[currentGun].rectTransform.sizeDelta += new Vector2(5, 5);
        yield return new WaitForSeconds(0.3f);

        gunImages[currentGun].rectTransform.sizeDelta -= new Vector2(5, 5);
        hotkeys[currentGun].rectTransform.sizeDelta -= new Vector2(5, 5);
        isChoosing = false;
    }

    private void HighlightGun(int index)
    {
        var notChosenColor = new Color(1, 1, 1, 0.5f);
        var chosenColor = Color.white;

        for (var i = 0; i < hotkeys.Count; i++)
        {
            if (i == index)
            {
                gunImages[i].color = chosenColor;
                hotkeys[i].color = chosenColor;
                gunImages[i].rectTransform.sizeDelta = highlightedSize + new Vector2(20, 0);
                hotkeys[i].rectTransform.sizeDelta = highlightedSize;
            }
            else
            {
                gunImages[i].color = notChosenColor;
                hotkeys[i].color = notChosenColor;
                gunImages[i].rectTransform.sizeDelta = defaultSize + new Vector2(20, 0);
                hotkeys[i].rectTransform.sizeDelta = defaultSize;
            }
        }
    }
}
