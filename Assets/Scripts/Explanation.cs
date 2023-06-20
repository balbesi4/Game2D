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
        "70-ые\n———–",
        "—оветские ученые проводили эксперименты с целью вывести новый вид €щериц, способных выживать в любых услови€х",
        "Ќо что-то пошло не так, и в итоге на свободу вырвалс€ крайне опасный вирус, поражающий сначала кожу, потом м€гкие ткани, затем нервные клетки, и по итогу человек превращалс€ в подобие монстра",
        "«а 10 суток было заражено 80% города, где была лаборатори€, за 44 дн€ 50% страны, за 5 мес€ца 52% всего мира",
        "Ќо главна€ проблема вируса - инкубационный период заражени€ длитс€ всего неделю",
        "„тобы спастись от этой заразы, люди пр€тались в бункерах",
        "¬ы берете под свое управление майора милиции ¬ладислава ѕопова. ќн, как и небольшое количество людей, успел войти в бункер до его полного закрыти€",
        "Ќо что-то пошло не так, он оказалс€ в центре вспышки",
        "» сейчас только путь в огонь был последним шансом выжить...",
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
