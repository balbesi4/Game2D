using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoctorCoat : MonoBehaviour
{
    public GameObject[] CutSceneObjects;
    public GameObject Dnevnik, Page;
    public Text CutSceneText;
    public GameObject LeftButton, RightButton, ExitPages;
    public Text LeftText, RightText;
    public SpriteRenderer Coat;
    public Sprite CoatSprite;
    public GameObject Player;
    public GameObject Trigger;
    public GameObject NotificationPanel;

    private int pageIndex = -1;
    private int phrazeIndex = 0;
    private bool isCutSceneGoing = false;

    private string[] phrazes = new[]
    {
        "�������... �� ���� ��������...",
        "������� ��� ��� ������ ��-�� ������� ��������������...",
        "�� �� ���� �� ����� ������ ������",
        "���� ����� ����������� ������� ���, ����� ��� ��������� ��� ��������",
        "��� � ��� ������������� ��� �������"
    };

    private string[] pages = new[]
    {
        "21.05.68\n�.�.�������\n\n���� ���������� �������, ��������� ��������� ������ � 4 ����. ���� ����������",
        "01.07.68\n�.�.�������\n\n� ������� ������� �� �������. ��������� �����, ������ ��� � �����, ��� ��� ��� ������",

        "14.07.68\n�.�.�������\n\n��� ������ ��� � �������� �� ��������, ������� ������ �������. ���� ��� ���������",
        "18.07.68\n�.�.�������\n\n����, � �������� ����� ������� ������ ����, ���� ������ �����...",

        "24.07.68\n�.�.�������\n\n�������� ���� ������, �������� �����, ����� ���� ��������� �������. ������� ����� ����� �������?",
        "3���,f6��f\n���-�����.\n\n��/6�01���?%6�3�80"
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
        if (pageIndex >= pages.Length - 1)
        {
            RightButton.SetActive(false);
            LeftButton.SetActive(true);
            ExitPages.SetActive(true);
            LeftText.text = pages[pageIndex - 1];
            RightText.text = pages[pageIndex];
        }
        else if (pageIndex == -1)
        {
            LeftButton.SetActive(false);
            LeftText.gameObject.SetActive(false);
            RightText.gameObject.SetActive(false);
            Page.SetActive(false);
            Dnevnik.SetActive(true);
        }
        else
        {
            LeftButton.SetActive(true);
            RightButton.SetActive(true);

            LeftText.text = pages[pageIndex - 1];
            RightText.text = pages[pageIndex];
        }
    }

    public void ChangePage(int direction)
    {
        pageIndex = pageIndex + 2 * direction;

        if (direction == -1) ExitPages.SetActive(false);
        else
        {
            LeftText.gameObject.SetActive(true);
            RightText.gameObject.SetActive(true);
            Dnevnik.SetActive(false);
            Page.SetActive(true);
        }
    }

    public void StartCutScene()
    {
        isCutSceneGoing = true;

        NotificationPanel.SetActive(false);
        Destroy(Page);
        Destroy(Dnevnik);
        Destroy(LeftButton);
        Destroy(RightButton);
        Destroy(LeftText.gameObject);
        Destroy(RightText.gameObject);
        Destroy(ExitPages);
        gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        foreach (var obj in CutSceneObjects)
            obj.SetActive(true);
        CutSceneText.gameObject.SetActive(true);
    }

    private void FinishCutScene()
    {
        Coat.sprite = CoatSprite;
        Destroy(Trigger);

        NotificationPanel.SetActive(true);
        Player.GetComponent<PlayerAnimation>().IsFreezed = false;
        Player.GetComponent<PlayerMovement>().IsFreezed = false;
        Player.GetComponent<ShootingControl>().IsFreezed = false;
        FindObjectOfType<InventoryManagement>().IsFreezed = false;

        Destroy(gameObject);
    }
}
