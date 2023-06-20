using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
    public GameObject MainMusic;
    public GameObject CutSceneMusic;
    public Sprite ManSprite;
    public Sprite DadSprite;

    private int currentPhrazeIndex = 0;
    private Vector3 startLevelPos;

    private readonly (bool Man, string Phraze)[] mainDialog = new[]
    {
        (true, "****, ****, ****. ЧТО ЭТО ЗА ******"),
        (false, "O, привет, а ты кто, че орешь?"),
        (true, "О ДЕД ЕМАЕ ТЫ ВООБЩЕ ВИДЕ ЧЕ ТАМ?"),
        (false, "Че? А, не, я спал с кайфом"),
        (true, "А В ЭТО ВРЕМЯ ТАМ ЛЮДИ ДРУГ ДРУГА ГРЫЗУТ"),
        (false, "Концерт Макана, что ли?"),
        (true, "ДА НЕ ДО ТВОИХ ШУТОЧЕК ЩА, ЮМОРИСТ ****. ЧЕ ДЕЛАТЬ ТО"),
        (false, "Ладно, успокойся ты, наш важный бумажный. А ща слушай меня внимательно"),
        (false, "То, что ты видел - вспышка вируса, и тебе очень повезло, что ты спасся"),
        (false, "Как сюда попал вирус - хз, но могу подсказать, кто знает"),
        (false, "Где-то на этаже должна быть моя внучка. Она принимала участие в программе по борьбе с вирусом"),
        (false, "Может она знает как спастись"),
        (true, "Окей, звучит как план, а может я лучше у тебя посижу?"),
        (false, "Во-первых, эта зона запривачена, а во-вторых, тут хаучика одному мне на неделю хватит"),
        (true, "Ну ладно, походу других вариантов у меня нет. А как она хоть выглядит"),
        (false, "Фотку возьми с тумбочки, там патроны для твоей пуколки лежат. И ни пуха ни пера"),
        (true, "Спасибо дед. К черту")
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
        MainMusic.SetActive(false);
        CutSceneMusic.SetActive(true);
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
        MainMusic.SetActive(true);
        CutSceneMusic.SetActive(false);

        Player.transform.position = startLevelPos;
        MainCamera.transform.position = startLevelPos;

        Destroy(gameObject);
    }
}
