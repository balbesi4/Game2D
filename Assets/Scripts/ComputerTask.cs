using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ComputerTask : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject ButtonPanel;
    public GameObject FinalPanel;
    public GameObject Level;
    public GameObject HintButton;
    public GameObject Hint;
    public GameObject Door;
    public GameObject Indicator;
    public GameObject[] Buttons1;
    public GameObject[] Buttons2;
    public GameObject[] Buttons3;
    public GameObject[] Buttons4;

    private bool[,] buttonValues;
    private bool completed, doorOpened, isEnding;
    private int userClicks;
    private GameObject[,] buttons;

    private void Awake()
    {
        buttons = new GameObject[4, 4];
        completed = false;
        doorOpened = false;
        isEnding = false;

        for (var i = 0; i < Buttons1.Length; i++)
            buttons[0, i] = Buttons1[i];
        for (var i = 0; i < Buttons2.Length; i++)
            buttons[1, i] = Buttons2[i];
        for (var i = 0; i < Buttons3.Length; i++)
            buttons[2, i] = Buttons3[i];
        for (var i = 0; i < Buttons4.Length; i++)
            buttons[3, i] = Buttons4[i];

        buttonValues = new bool[4, 4]
        {
            { true, false, false, true },
            { true, false, true, true },
            { false, true, false, true },
            { true, false, false, true }
        };

        FinalPanel.GetComponentInChildren<Image>().color = Color.Lerp(Color.red, Color.white, 0.7f);
        FinalPanel.GetComponentsInChildren<Text>().Last().text = "OFF";
    }

    private void Update()
    {
        if (!completed && CheckCompletion())
            completed = true;
        else if (completed && !isEnding && ButtonPanel.activeSelf)
            StartCoroutine(ShowCompletion());
    }

    private IEnumerator ShowCompletion()
    {
        isEnding = true;

        for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            {
                buttons[i, j].GetComponent<Image>().color = new Color(0.25f, 0.75f, 0.3f, 1);
                yield return new WaitForSeconds(0.1f);
            }

        ButtonPanel.SetActive(false);
        FinalPanel.SetActive(true);
        isEnding = false;
    }

    private bool CheckCompletion()
    {
        var trues = 0;
        var falses = 0;

        for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            {
                if (buttonValues[i, j])
                    trues++;
                else
                    falses++;
            }

        return trues == 16 || falses == 16;
    }

    public void CloseComputer()
    {
        if (isEnding) return;

        Level.SetActive(true);
        gameObject.SetActive(false);
    }

    public void EnterTask()
    {
        StartPanel.SetActive(false);
        ButtonPanel.SetActive(true);
    }

    public void ChangeButtonsColor(int index)
    {
        if (isEnding) return;

        var x = index / 4;
        var y = index % 4;

        for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            {
                if (x == i || y == j)
                {
                    buttonValues[i, j] = !buttonValues[i, j];
                    var button = buttons[i, j];
                    var currentColor = button.GetComponent<Image>().color;
                    button.GetComponent<Image>().color = currentColor == Color.white ? Color.black : Color.white;
                }
            }

        userClicks++;
        CheckHint();
    }

    private void CheckHint()
    {
        if (userClicks == 20)
            HintButton.SetActive(true);
    }

    public void ShowHint()
    {
        if (isEnding) return;

        if (Hint.activeSelf)
            Hint.SetActive(false);
        else
            Hint.SetActive(true);
    }

    public void OpenCloseDoor()
    {
        Door.SetActive(doorOpened);
        doorOpened = !doorOpened;
        FinalPanel.GetComponentInChildren<Image>().color = doorOpened ? new Color(0.25f, 0.75f, 0.3f, 1) : Color.Lerp(Color.red, Color.white, 0.7f);
        FinalPanel.GetComponentsInChildren<Text>().Last().text = doorOpened ? "ON" : "OFF";
        Indicator.GetComponent<SpriteRenderer>().color = doorOpened ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
    }
}
