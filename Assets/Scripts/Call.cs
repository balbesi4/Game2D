using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Call : MonoBehaviour
{
    public Text DialogText;
    public Image Portrait;
    public Sprite Man, Girl;
    public GameObject[] CallObjects;

    private int dialogIndex = 0;
    private int phrazeIndex = 0;
    private bool isOn = true;

    private (bool Man, string Phraze)[] firstCall = new[]
    {
        (true, "Я залил топливо в машину, тут какой-то здоровый зомби"),
        (false, "НЕ ДЕРИСЬ С НИМ!!! ПРОСТО УХОДИ!!!")
    };
    private (bool Man, string Phraze)[] secondCall = new[]
    {
        (true, "Твою за ногу, какой пароль???"),
        (false, "СИ.. ЗЕЛ..ЫЙ..")
    };

    private void Update()
    {
        if (!isOn) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (phrazeIndex == firstCall.Length - 1 && dialogIndex == 0)
            {
                phrazeIndex = 0;
                dialogIndex = 1;
                Off();
            }
            else if (dialogIndex == 1)
            {
                phrazeIndex = phrazeIndex + 1 > secondCall.Length - 1 ? secondCall.Length - 1 : phrazeIndex + 1;
            }
            else
            {
                phrazeIndex++;
            }
        }

        var currentDialog = dialogIndex == 0 ? firstCall : secondCall;
        DialogText.text = currentDialog[phrazeIndex].Phraze;
        Portrait.sprite = currentDialog[phrazeIndex].Man ? Man : Girl;
    }

    public void On()
    {
        if (dialogIndex == 0)
        {
            dialogIndex++;
            phrazeIndex = 0;
        }

        isOn = true;
        foreach (var obj in CallObjects)
            obj.SetActive(true);
    }

    private void Off()
    {
        isOn = false;
        foreach (var obj in CallObjects)
            obj.SetActive(false);
    }
}
