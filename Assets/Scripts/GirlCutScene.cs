using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GirlCutScene : MonoBehaviour
{
    public DoorManagement[] Room;
    public GameObject CutSceneTrigger;
    public Image Portrait;
    public Text DialogText;
    public GameObject FirstCutScenePanel;
    public GameObject NotifcationPanel;
    public GameObject PlayerPanelUI;
    public GameObject Girl;
    public GameObject GrayWall;
    public GameObject GreenKeyDrop;
    public Sprite ManSprite;
    public Sprite GirlSprite;
    public GameObject EmployeeCard;
    public GameObject WireTaskDetail1;

    private int cutSceneIndex = 0;
    private int currentPhrazeIndex = 0;
    private bool isCutSceneGoing;
    private (bool Man, string Phraze)[][] cutSceneDialogs;

    private readonly (bool Man, string Phraze)[] firstDialog = new[]
    {
        (false, "Привет я бебра азазазза"),
        (true, "Привет ДАНУНА РЯЛЬНА"),
        (false, "Не я тебя обманула кек"),
        (true, "Ну и иди в попу блин((((((")
    };

    private readonly (bool Man, string Phraze)[] secondDialog = new[]
    {
        (false, "Ура ты крутой"),
        (true, "Спасибо ура"),
        (false, "Тебе надо спасти человечество"),
        (true, "Базарчик)")
    };

    private readonly (bool Man, string Phraze)[] reserveDialog = new[]
    {
        (false, "Если вы что-то забыли, то вы дурачок КЕК")
    };

    private void Start()
    {
        isCutSceneGoing = false;
        cutSceneDialogs = new[] { firstDialog, secondDialog, reserveDialog };
    }

    private void Update()
    {
        if (cutSceneIndex < Room.Length && Room[cutSceneIndex].ZombieCount == 0 && !isCutSceneGoing)
        {
            CutSceneTrigger.SetActive(true);
            if (cutSceneIndex == 0)
                GrayWall.SetActive(false);
        }
        else if (isCutSceneGoing)
        {
            var currentDialog = cutSceneDialogs[cutSceneIndex];
            if (currentDialog[currentPhrazeIndex].Man)
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
                Portrait.sprite = GirlSprite;
                Portrait.rectTransform.anchorMax = new Vector2(1, 0);
                Portrait.rectTransform.anchorMin = new Vector2(1, 0);
                Portrait.rectTransform.anchoredPosition = new Vector3(-200, 180, 0);
                Portrait.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                DialogText.rectTransform.offsetMax = new Vector2(-360, 0);
                DialogText.rectTransform.offsetMin = new Vector2(160, 0);
            }
            DialogText.text = currentDialog[currentPhrazeIndex].Phraze;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPhrazeIndex == currentDialog.Length - 1)
                    FinishGirlCutScene();
                else
                    currentPhrazeIndex++;
            }
        }
    }

    public void StartGirlCutScene()
    {
        isCutSceneGoing = true;
        FindObjectOfType<InventoryManagement>().IsFreezed = true;
        FindObjectOfType<PlayerMovement>().IsFreezed = true;
        FindObjectOfType<ShootingControl>().IsFreezed = true;
        FindObjectOfType<PlayerAnimation>().IsFreezed = true;
        NotifcationPanel.SetActive(false);
        PlayerPanelUI.SetActive(false);
        FirstCutScenePanel.SetActive(true);
    }

    private void FinishGirlCutScene()
    {
        if (cutSceneIndex == 0)
        {
            GrayWall.SetActive(true);

            Girl.transform.position -= new Vector3(4.5f, -6.1f, 0);
            CutSceneTrigger.transform.position -= new Vector3(5.7f, -6.6f, 0);
            CutSceneTrigger.transform.rotation = Quaternion.Euler(0, 0, 90);
            CutSceneTrigger.transform.localScale = new Vector3(1.25f,
                CutSceneTrigger.transform.localScale.y,
                CutSceneTrigger.transform.localScale.z);
            CutSceneTrigger.SetActive(false);

            Instantiate(GreenKeyDrop, Girl.GetComponentsInParent<Transform>()[1]);
        }
        else if (cutSceneIndex == 1)
        {
            Instantiate(EmployeeCard, Girl.transform.position - new Vector3(3, 1f, 0), Quaternion.identity, Girl.GetComponentsInParent<Transform>()[1]);
            var detail = Instantiate(WireTaskDetail1, Girl.transform.position - new Vector3(3, -1f, 0), Quaternion.identity, Girl.GetComponentsInParent<Transform>()[1]);
            detail.GetComponent<WireTaskDetail>().Index = 1;
        }

        FindObjectOfType<InventoryManagement>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        FirstCutScenePanel.SetActive(false);
        NotifcationPanel.SetActive(true);
        PlayerPanelUI.SetActive(true);

        cutSceneIndex = cutSceneIndex == cutSceneDialogs.Length - 1
            ? cutSceneIndex
            : cutSceneIndex + 1;

        currentPhrazeIndex = 0;
        isCutSceneGoing = false;
    }
}
