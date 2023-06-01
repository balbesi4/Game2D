using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public GameObject MainMusic;
    public GameObject CutSceneMusic;
    public GameObject GirlScream;

    private int cutSceneIndex = 0;
    private int currentPhrazeIndex = 0;
    private bool isCutSceneGoing;
    private (bool Man, string Phraze)[][] cutSceneDialogs;

    private readonly (bool Man, string Phraze)[] firstDialog = new[]
    {
        (true, "Так, давай по делу. ты *вумен нейм*?"),
        (false, "Ну да, а откуд..."),
        (true, "Мне твой дед сказал, как тебя найти, сказал, что ты поможешь выбраться"),
        (false, "Слава богу хоть он живой, а родителей моих не видел?"),
        (true, "Прости, но нет, но может с ними все ок"),
        (false, "Наверно, но верить в это сложно... Ладно, про того как уйти - уже никак"),
        (false, "Проектировщики бункера сделали так, что все выходы закупориваются и ни уйти, ни выйти"),
        (false, "Так что мы да, мы в ловушке"),
        (true, "И че даже никакой вентиляции или другого способа выбраться"),
        (false, "Нууууу... есть конечно один, он может не сработать, но сначала освоборди меня"),
        (false, "Кароче, на кухне сидит бабиджон, и я не могу выбраться"),
        (false, "Туда ты можешь попасть через склад, вот ключ от него"),
        (false, "Так что иди помоги мне выбраться и мы придумаем че делать"),
        (true, "Ок я погнал, начинай собирать вещи")
    };

    private readonly (bool Man, string Phraze)[] secondDialog = new[]
    {
        (false, "БОЛЬШОЕ ТЕБЕ СПАСИБО. извини, но где ты так научился стрелять"),
        (true, "Я СОПРовец, нас стрельбе при устройстве учат"),
        (false, "А, тогда хорошо, просто я уже бояться тебя начала"),
        (true, "Не бойся, я вообще кот. го к делу - как выбраться?"),
        (false, "А, да, точно. Кароче, как уже сказала - никак, но я была в команде по уничтожению вируса"),
        (false, "Сначала мы хотели приумать антивирус, но из за генных мутаций ниче не помогало"),
        (false, "По итогу мы решили сделать безумную идею и она почти получилась"),
        (true, "Не томи"),
        (false, "Машина времени"),
        (true, "Машина времени?"),
        (false, "Да. Там конечно не делориан, то тоже ниче"),
        (false, "С ее помощью мы хотели отправиться в прошлое и не дать вирусу заразить человечество"),
        (true, "И, как понимаю, идея провалилась"),
        (false, "Почти, мы просто не успели ее запустить, но ща я встретила тебя, так что у тебя должно получиться"),
        (true, "А че я то?"),
        (false, "У тебя физ подготовка лучше, у тебя больше шансов дойти"),
        (true, "Ну тоже верно. Окей, я уже стал как-будто героем игры какой-то. Куда идти?"),
        (false, "Смари, тут где то должна быть дорога до второй части бункера"),
        (false, "Она создавалась для правительственных чуваков, им где-то там оно должно быть"),
        (false, "Попасть туда можно через портал, но к нему нужно электричество"),
        (false, "Как запустить машину, ты поймешь сам"),
        (false, "Отправляешься на 2 месяца назад, пока вирус не распространился"),
        (false, "Звонишь мне и обо всем рассказываешь"),
        (false, "Понял?"),
        (true, "Ну, +-"),
        (false, "Хорошо, удачи тебе. И запомни - сначала синий потом зеленый"),
        (false, "СИНИЙ ЗЕЛЕНЫЙ")
    };

    private readonly (bool Man, string Phraze)[] reserveDialog = new[]
    {
        (false, "Что бы запустить электричество тебе 3 детали надо найти")
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
            {
                GirlScream.SetActive(true);
                GrayWall.SetActive(false);
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
        GirlScream.SetActive(false);
        FindObjectOfType<InventoryManagement>().IsFreezed = true;
        FindObjectOfType<PlayerMovement>().IsFreezed = true;
        FindObjectOfType<ShootingControl>().IsFreezed = true;
        FindObjectOfType<PlayerAnimation>().IsFreezed = true;
        NotifcationPanel.SetActive(false);
        PlayerPanelUI.SetActive(false);
        FirstCutScenePanel.SetActive(true);
        MainMusic.SetActive(false);
        CutSceneMusic.SetActive(true);
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
        MainMusic.SetActive(true);
        CutSceneMusic.SetActive(false);

        cutSceneIndex = cutSceneIndex == cutSceneDialogs.Length - 1
            ? cutSceneIndex
            : cutSceneIndex + 1;

        currentPhrazeIndex = 0;
        isCutSceneGoing = false;
    }
}
