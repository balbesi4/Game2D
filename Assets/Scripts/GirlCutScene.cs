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

    private int cutSceneIndex = 0;
    private int currentPhrazeIndex = 0;
    private bool isCutSceneGoing, isGirlOut;
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

    private void Start()
    {
        isCutSceneGoing = false;
        isGirlOut = false;
        cutSceneDialogs = new[] { firstDialog, secondDialog };
    }

    private void Update()
    {
        if (cutSceneIndex == cutSceneDialogs.Length)
            Destroy(GetComponent<GirlCutScene>());
        else if (Room[cutSceneIndex].ZombieCount == 0 && !isCutSceneGoing)
        {
            CutSceneTrigger.SetActive(true);
            if (cutSceneIndex == 0)
                GrayWall.SetActive(false);
            else if (cutSceneIndex == 1 && !isGirlOut)
            {
                isGirlOut = true;
                Girl.transform.position -= new Vector3(1.5f, 0);
            }
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

    public void FinishGirlCutScene()
    {
        if (cutSceneIndex == 0)
        {
            GrayWall.SetActive(true);

            Girl.transform.position -= new Vector3(4.5f, -6.1f, 0);
            CutSceneTrigger.transform.position -= new Vector3(6.5f, -6.8f, 0);
            CutSceneTrigger.transform.rotation = Quaternion.Euler(0, 0, 90);

            Instantiate(GreenKeyDrop, Girl.GetComponentsInParent<Transform>()[1]);
        }
        else
        {
            Destroy(GrayWall);
            Destroy(CutSceneTrigger);
        }

        FindObjectOfType<InventoryManagement>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        FirstCutScenePanel.SetActive(false);
        NotifcationPanel.SetActive(true);
        PlayerPanelUI.SetActive(true);

        cutSceneIndex++;
        currentPhrazeIndex = 0;
        CutSceneTrigger.SetActive(false);
        isCutSceneGoing = false;
    }
}
