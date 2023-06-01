using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorCoat : MonoBehaviour
{
    public GameObject[] CutSceneObjects;
    public Sprite[] Pages;
    public GameObject Dnevnik, Page;
    public Text CutSceneText;
    public GameObject LeftButton, RightButton, ExitPages;
    public Text LeftText, RightText;
    public SpriteRenderer Coat;
    public Sprite CoatSprite;
    public GameObject Trigger;
    public GameObject Player;

    private int pageIndex = 0;
    private int phrazeIndex = 0;
    private bool isCutSceneGoing = false;

    private string[] phrazes = new[]
    {
        "Неужели... Не могу поверить...",
        "Неужели это все правда из-за Владимира Александровича...",
        "Он же один из самых крутых учёных",
        "Надо сохранить этот дневник себе и показать Анне при встрече",
        "Надеюсь, что в прошлом какая-то часть записей останется"
    };

    private string[] pages = new[]
    {
        "21.05.68\nВеду разработку вакцины, усиляющей человека в 4 раза",
        "01.07.68\nЯ испытал вакцину на кролике, но забыл, что она для людей",

        "14.07.68\nВклоллол сее вкацииын",
        ""
    };

    private void Update()
    {
        if (!isCutSceneGoing)
        {
            ManagePages();
        }
        else
        {
            CutSceneText.text = phrazes[phrazeIndex];

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (phrazeIndex == phrazes.Length - 1)
                    FinishCutScene();
                else
                    phrazeIndex++;
            }
        }
    }

    private void ManagePages()
    {
        if (pageIndex == 0)
            LeftButton.SetActive(false);
        else if (pageIndex == Pages.Length - 1)
        {
            RightButton.SetActive(false);
            ExitPages.SetActive(true);
        }
    }

    public void StartCutScene()
    {
        isCutSceneGoing = true;

        Destroy(Page);
        Destroy(Dnevnik);
        Destroy(LeftButton);
        Destroy(RightButton);
        Destroy(LeftText.gameObject);
        Destroy(RightText.gameObject);
        Destroy(ExitPages);

        foreach (var obj in CutSceneObjects)
            obj.SetActive(true);
        CutSceneText.gameObject.SetActive(true);
    }

    private void FinishCutScene()
    {
        Coat.sprite = CoatSprite;
        Destroy(Trigger);

        Player.GetComponent<PlayerAnimation>().IsFreezed = false;
        Player.GetComponent<PlayerMovement>().IsFreezed = false;
        Player.GetComponent<ShootingControl>().IsFreezed = false;
        FindObjectOfType<InventoryManagement>().IsFreezed = false;

        Destroy(gameObject);
    }
}
