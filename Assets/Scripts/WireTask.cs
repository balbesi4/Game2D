using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WireTask : MonoBehaviour
{
    public GameObject[] Buttons;
    public GameObject InputButton;
    public GameObject Portal;
    public Text NoDetailsText;
    public GameObject Details;
    public GameObject Trigger;
    public GameObject Transformator;
    public GameObject Game;
    private bool isEnding, canBePassed;

    public int DetailsCount { get; set; }

    private int[] buttonValues;

    private void Start()
    {
        isEnding = false;
        canBePassed = false;

        buttonValues = new[]
        {
            3, 1, 0, 0,
            0, 0, 3, 0,
            2, 0, 2, 0,
            2, 1, 0, 1
        };
    }

    private void Update()
    {
        if (DetailsCount == 3)
        {
            NoDetailsText.gameObject.SetActive(false);
            InputButton.SetActive(true);
        }
        else if (!isEnding && buttonValues.All(button => button == 0))
            StartCoroutine(ShowCompletion());
    }

    private IEnumerator ShowCompletion()
    {
        isEnding = true;
        var oldColor = NoDetailsText.color;
        NoDetailsText.text = "Проводка починена";
        NoDetailsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        NoDetailsText.color = new Color(0.25f, 0.75f, 0.3f, 1);
        yield return new WaitForSeconds(0.1f);
        NoDetailsText.color = oldColor;
        yield return new WaitForSeconds(0.1f);
        NoDetailsText.color = new Color(0.25f, 0.75f, 0.3f, 1);
        yield return new WaitForSeconds(0.1f);
        NoDetailsText.color = oldColor;
        yield return new WaitForSeconds(0.1f);


        Portal.SetActive(true);
        isEnding = false;
        Game.SetActive(true);
        Transformator.GetComponent<Animator>().SetLayerWeight(1, 1);
        Destroy(Trigger);
        Destroy(gameObject);
    }

    public void ChangeButtonValue(int index)
    {
        if (isEnding || !canBePassed || index == 2 || index == 9 || index == 14) return;

        if (index == 1 || index == 4)
            buttonValues[index] = (buttonValues[index] + 1) % 2;
        else if (index == 11)
            buttonValues[index] = 0;
        else
            buttonValues[index] = (buttonValues[index] + 1) % 4;
        
        Buttons[index].transform.Rotate(0, 0, 90);
    }

    public void EnablePassing()
    {
        DetailsCount = 0;
        FindObjectOfType<InventoryManagement>().Clear();
        Details.SetActive(true);
        canBePassed = true;
        Destroy(InputButton);
    }

    public void CloseTask()
    {
        Game.SetActive(true);
        gameObject.SetActive(false);
    }
}
