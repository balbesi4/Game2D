using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VaultTask : MonoBehaviour
{
    public GameObject Game;
    public Camera MainCamera;
    public GameObject VaultCamera;
    public Text PasswordText;
    public GameObject VaultTrigger;
    public AudioClip ClickSound;

    private const string Password = "591487";
    private StringBuilder inputPassword = new StringBuilder();
    private Color textColor;
    private bool canBeClosed;  

    private void Start()
    {
        textColor = PasswordText.color;
        canBeClosed = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && canBeClosed)
        {
            CloseTask();
        }
    }

    public void AddPasswordNumber(string number)
    {
        if (inputPassword.Length >= Password.Length) return;
        inputPassword.Append(number);
        AudioSource.PlayClipAtPoint(ClickSound, VaultCamera.transform.position);
        PasswordText.text += "*";
    }

    public void ErasePassword()
    {
        PasswordText.text = "";
        inputPassword.Clear();
    }

    public void ConfirmPassword()
    {
        if (inputPassword.ToString() == Password)
        {
            canBeClosed = false;
            VaultTrigger.GetComponent<VaultController>().IsPassed = true;
            StartCoroutine(RightPasswordAnimation());
        }

        else
        {
            canBeClosed = false;
            StartCoroutine(WrongPasswordAnimation());
        }
    }

    private IEnumerator WrongPasswordAnimation()
    {
        PasswordText.color = Color.red;
        yield return new WaitForSeconds(1);
        PasswordText.color = textColor;
        ErasePassword();
        canBeClosed = true;
    }

    private IEnumerator RightPasswordAnimation()
    {
        for (var i = 0; i < 3; i++)
        {
            PasswordText.color = Color.Lerp(Color.white, Color.green, 0.5f);
            yield return new WaitForSeconds(0.3f);
            PasswordText.color = textColor;
            yield return new WaitForSeconds(0.3f);
        }
        CloseTask();
        Destroy(gameObject);
    }

    private void CloseTask()
    {
        gameObject.SetActive(false);
        Game.SetActive(true);
        MainCamera.gameObject.SetActive(true);
    }
}
