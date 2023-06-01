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

    private int pageIndex = 0;
    private bool isCutSceneGoing = false;

    private string[] phrazes = new[]
    {
        "�������... �� ���� ��������...",
        "������� ��� ��� ������ ��-�� ��������� ��������������...",
        "�� �� ���� �� ����� ������ ������",
        "���� ��������� ���� ������� ���� � �������� ���� ��� �������",
        "�������, ��� � ������� �����-�� ����� ������� ���������"
    };

    private void Update()
    {
        if (!isCutSceneGoing)
        {
            ManagePages();
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


    }
}
