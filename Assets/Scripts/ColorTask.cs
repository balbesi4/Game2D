using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTask : MonoBehaviour
{
    public GameObject Level;
    public GameObject Trigger;
    public GameObject PetrolCan;
    public GameObject[] Bars;
    public Text[] Texts;

    private int[] status;
    private bool isShowng;

    private void Awake()
    {
        status = new[] { 2, 1, 4, 3 };
        isShowng = false;
    }

    private void Update()
    {
        var right = 0;

        for (var i = 1; i <= status.Length; i++)
            if (status[i - 1] == i)
                right++;

        if (right == 4 && !isShowng)
            StartCoroutine(ShowCompletion());
    }

    public void ChangeBars(int buttonIndex)
    {
        if (isShowng) return;
        StartCoroutine(ShowChanging(buttonIndex));
        (status[buttonIndex - 1], status[buttonIndex]) = (status[buttonIndex], status[buttonIndex - 1]);
        (Bars[buttonIndex], Bars[buttonIndex - 1]) = (Bars[buttonIndex - 1], Bars[buttonIndex]);
    }

    private IEnumerator ShowChanging(int buttonIndex)
    {
        isShowng = true;

        var first = Bars[buttonIndex - 1];
        var second = Bars[buttonIndex];

        var firstPos = first.transform.localPosition;
        var secondPos = second.transform.localPosition;

        while (first.transform.localPosition != secondPos)
        {
            first.transform.localPosition += new Vector3(5, 0);
            second.transform.localPosition -= new Vector3(5, 0);
            yield return new WaitForSeconds(0.1f);
        }

        isShowng = false;
    }

    private IEnumerator ShowCompletion()
    {
        isShowng = true;
        var oldColor = Texts[0].color;

        foreach (var text in Texts)
            text.color = new Color(0.25f, 0.75f, 0.3f, 1);

        yield return new WaitForSeconds(0.5f);

        foreach (var text in Texts)
            text.color = oldColor;

        yield return new WaitForSeconds(0.5f);

        foreach (var text in Texts)
            text.color = new Color(0.25f, 0.75f, 0.3f, 1);

        yield return new WaitForSeconds(0.5f);

        foreach (var text in Texts)
            text.color = oldColor;

        yield return new WaitForSeconds(0.5f);

        Level.SetActive(true);
        Instantiate(PetrolCan, Trigger.transform.position - new Vector3(0.5f, 0), Quaternion.identity, Trigger.GetComponentsInParent<Transform>()[1]);
        Destroy(Trigger);
        Destroy(gameObject);
    }

    public void CloseTask()
    {
        if (isShowng) return;

        Level.SetActive(true);
        gameObject.SetActive(false);
    }
}
