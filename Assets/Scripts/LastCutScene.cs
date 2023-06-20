using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastCutScene : MonoBehaviour
{
    public Image Portrait;
    public Text DialogText;
    public Sprite ManSprite;
    public Sprite GirlSprite;

    private int currentPhrazeIndex = 0;
    private bool isFinished = false;

    private readonly (bool Man, string Phraze)[] mainDialog = new[]
    {
        (true, "*��� ��� ���*"),
        (false, "A���"),
        (true, "�����?"),
        (false, "������ ����, �� ���?"),
        (true, "�� ���� �� ������, � ������ ���� ��� ����� ��������� ������, �� ������, ����������"),
        (true, "�������� �������� � ��������� ������� ������� �������������� ��������, � �������� ��� ��������"),
        (true, "�� ������� �����������"),
        (false, "�������, �� ���������, ��� �� ��������?"),
        (true, "� �������, � �� ��� ��� ������ ���� �� ���������"),
        (true, "�� �� ������, ��� ����������, ����� � ����� ������, �������� ��� � ������� ���� ����"),
        (false, "������� �������, ������������, ����������, � ����� �������"),
        (true, "*��� ��� ���*"),
        (true, "*���� ��������*"),
        (true, "*� ������ ��� ������� ����� �����, �� �������� ������� ���...*"),
        (true, "...")
    };

    private void Update()
    {
        if (!isFinished)
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
                Portrait.sprite = GirlSprite;
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
        else
        {
            DialogText.text = "�� ������ ����, �����������!";
            DialogText.rectTransform.offsetMax = new Vector3(-260, 236.77f);
            DialogText.rectTransform.offsetMin = new Vector3(260, 236.77f);
            DialogText.alignment = TextAnchor.MiddleCenter;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerPrefs.SetInt("Scene to load", 0);
                SceneManager.LoadScene((int)Scene.MainMenu);
            }
        }
    }

    public void FinishDadCutScene()
    {
        isFinished = true;
        Portrait.gameObject.SetActive(false);
    }
}
