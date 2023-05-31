using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GranddadCutScene : MonoBehaviour
{
    public Image Portrait;
    public Text DialogText;
    public GameObject CutScenePanel;
    public GameObject NotifcationPanel;
    public GameObject PlayerPanelUI;
    public GameObject Player;
    public GameObject MainCamera;
    public GameObject Music;
    public Sprite ManSprite;
    public Sprite DadSprite;

    private int currentPhrazeIndex = 0;
    private Vector3 startLevelPos;

    private readonly (bool Man, string Phraze)[] mainDialog = new[]
    {
        (false, "О привет а я тебя знаю"),
        (true, "Ты че дед с дуба рухнул ты кто"),
        (false, "Эй поуважительнее я воевал"),
        (true, "Сорян не признал"),
        (false, "Короче надо от зомби всех спасти делаешь?"),
        (true, "Халява скринь плесень")
    };

    public void Start()
    {
        FindObjectOfType<InventoryManagement>().IsFreezed = true;
        FindObjectOfType<PlayerMovement>().IsFreezed = true;
        FindObjectOfType<ShootingControl>().IsFreezed = true;
        FindObjectOfType<PlayerAnimation>().IsFreezed = true;
        NotifcationPanel.SetActive(false);
        PlayerPanelUI.SetActive(false);
        CutScenePanel.SetActive(true);
        Music.SetActive(false);
        startLevelPos = new Vector3(4, 2.5f, Player.transform.position.z);
    }

    private void Update()
    {
        var currentPhraze = mainDialog[currentPhrazeIndex];
        if (currentPhraze.Man)
        {
            Portrait.sprite = ManSprite;
            Portrait.rectTransform.anchorMax = new Vector2(0, 0);
            Portrait.rectTransform.anchorMin = new Vector2(0, 0);
            Portrait.rectTransform.anchoredPosition = new Vector3(200, 180, 0);
            Portrait.rectTransform.rotation = Quaternion.Euler(0, 180, 0);
            DialogText.rectTransform.offsetMax = new Vector2(-160, 0);
            DialogText.rectTransform.offsetMin = new Vector2(360, 0);
        }
        else
        {
            Portrait.sprite = DadSprite;
            Portrait.rectTransform.anchorMax = new Vector2(1, 0);
            Portrait.rectTransform.anchorMin = new Vector2(1, 0);
            Portrait.rectTransform.anchoredPosition = new Vector3(-200, 180, 0);
            Portrait.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            DialogText.rectTransform.offsetMax = new Vector2(-360, 0);
            DialogText.rectTransform.offsetMin = new Vector2(160, 0);
        }
        DialogText.text = currentPhraze.Phraze;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPhrazeIndex == mainDialog.Length - 1)
                FinishDadCutScene();
            else
                currentPhrazeIndex++;
        }
    }

    public void FinishDadCutScene()
    {
        FindObjectOfType<InventoryManagement>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        CutScenePanel.SetActive(false);
        NotifcationPanel.SetActive(true);
        PlayerPanelUI.SetActive(true);
        Music.SetActive(true);

        Player.transform.position = startLevelPos;
        MainCamera.transform.position = startLevelPos;

        Destroy(gameObject);
    }
}
