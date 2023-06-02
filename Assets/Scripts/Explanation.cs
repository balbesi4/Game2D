using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Explanation : MonoBehaviour
{
    public Text MainText;

    private int phrazeIndex = 0;

    private string[] phrazes = new[]
    {
        "70-ые\nСССР",
        "Советские ученые проводили эксперименты с целью вывести новый вид ящериц, способных выживать в любых условиях",
        "Но что-то пошло не так, и в итоге на свободу вырвался крайне опасный вирус, поражающий сначала кожу, потом мягкие ткани, затем нервные клетки, и по итогу человек превращался в подобие монстра",
        "За 10 суток было заражено 80% города, где была лабораторя, за 44 дня 50% страны, за 5 месяца 52% всего мира",
        "Но главная проблема вируса - инкубационный период заражения - неделя",
        "Чтобы спастись от этой заразы, люди прятались в бункерах",
        "Вы берете под свое управление майора милиции Владислава Попова. Он, как и небольшое количество людей, успел войти в бункер до его полного закрытия",
        "Но что-то пошло не так, он оказался в центре вспышки",
        "И сейчас только путь в огонь был последним шансом выжить...",
    };

    private void Update()
    {
        MainText.text = phrazes[phrazeIndex];

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (phrazeIndex < phrazes.Length - 1)
                phrazeIndex++;
            else
            {
                PlayerPrefs.SetInt("Scene to load", (int)Scene.TutorialBunker);
                SceneManager.LoadScene((int)Scene.TutorialBunker);
            }
        }
    }
}
