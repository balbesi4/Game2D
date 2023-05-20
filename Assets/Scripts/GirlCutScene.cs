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
    public GameObject Girl;
    public GameObject GrayWall;
    public GameObject GreenKeyDrop;
    public Sprite ManSprite;
    public Sprite GirlSprite;

    private int cutSceneIndex = 0;
    private int currentPhrazeIndex = 0;
    private bool isCutSceneGoing;
    private readonly (bool Man, string Phraze)[] firstDialog = new[]
    {
        (false, "Привет я бебра азазазза"),
        (true, "Привет ДАНУНА РЯЛЬНА"),
        (false, "Не я тебя обманула кек"),
        (true, "Ну и иди в попу блин((((((")
    };

    private void Start()
    {
        isCutSceneGoing = false;
    }

    private void Update()
    {
        if (Room[cutSceneIndex].ZombieCount == 0 && !isCutSceneGoing)
        {
            CutSceneTrigger.SetActive(true);
            if (cutSceneIndex == 0)
                GrayWall.SetActive(false);
        }
        else if (isCutSceneGoing && cutSceneIndex == 0)
        {
            Portrait.sprite = firstDialog[currentPhrazeIndex].Man ? ManSprite : GirlSprite;
            DialogText.text = firstDialog[currentPhrazeIndex].Phraze;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentPhrazeIndex == firstDialog.Length - 1)
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
        FirstCutScenePanel.SetActive(true);
    }

    public void FinishGirlCutScene()
    {
        GrayWall.SetActive(true);

        Girl.transform.position -= new Vector3(4.5f, -6.1f, 0);
        CutSceneTrigger.transform.position -= new Vector3(5.5f, -6.8f, 0);
        CutSceneTrigger.transform.rotation = Quaternion.Euler(0, 0, 90);

        if (cutSceneIndex == 0)
            Instantiate(GreenKeyDrop, Girl.GetComponentsInParent<Transform>()[1]);

        FindObjectOfType<InventoryManagement>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        FirstCutScenePanel.SetActive(false);
        NotifcationPanel.SetActive(true);

        cutSceneIndex++;
        currentPhrazeIndex = 0;
        CutSceneTrigger.SetActive(false);
        isCutSceneGoing = false;
    }
}
