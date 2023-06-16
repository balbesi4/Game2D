using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeMachineController : MonoBehaviour
{
    public Text NotificationText;
    public GameObject HotkeyF;
    public GameObject MainCamera;
    public GameObject TaskPanel;
    public GameObject BigDoor, OtherDoors;
    public GameObject BossZombie;
    public bool IsPetrolGrabbed, IsTaskPassed;
    public GameObject MainSound;
    public GameObject BossSound;
    public GameObject MachineSound;
    public GameObject SirenSound;
    public GameObject DoorSound;
    public GameObject CoatTrigger;
    public GameObject CallPanel;

    private bool isPetrolUsed, isShowingBoss;

    private void Start()
    {
        isShowingBoss = false;
        isPetrolUsed = false;
        IsPetrolGrabbed = false;
        IsTaskPassed = false;
    }

    public void CheckPetrolCan()
    {
        if (isShowingBoss) return;

        if (IsPetrolGrabbed)
        {
            HotkeyF.SetActive(true);

            if (!isPetrolUsed)
                NotificationText.text = "Залить топливо";
            else if (isPetrolUsed && !IsTaskPassed)
                NotificationText.text = "Открыть панель управления";
            else
                NotificationText.text = "Зайти в машину времени";

            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
                OpenTimeMashine();
        }
        else
        {
            NotificationText.text = "Нужен топливный бак";
            NotificationText.color = Color.white;
            NotificationText.gameObject.SetActive(true);
        }
    }

    private void OpenTimeMashine()
    {
        if (!isPetrolUsed)
            StartCoroutine(UsePetrol());
        else if (isPetrolUsed && !IsTaskPassed)
        {
            FindObjectOfType<PlayerAnimation>().IsFreezed = true;
            FindObjectOfType<PlayerMovement>().IsFreezed = true;
            FindObjectOfType<ShootingControl>().IsFreezed = true;
            FindObjectOfType<InventoryManagement>().IsFreezed = true;

            TaskPanel.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Scene to load", (int)Scene.FinalScene);
            SceneManager.LoadScene((int)Scene.FinalScene);
        }
    }

    private IEnumerator UsePetrol()
    {
        isShowingBoss = true;
        isPetrolUsed = true;
        Stop();

        FindObjectOfType<PlayerAnimation>().IsFreezed = true;
        FindObjectOfType<PlayerMovement>().IsFreezed = true;
        FindObjectOfType<ShootingControl>().IsFreezed = true;
        FindObjectOfType<InventoryManagement>().IsFreezed = true;

        //звук машины времени + мб звук сирены
        MainSound.SetActive(false);
        MachineSound.SetActive(true);
        SirenSound.SetActive(true);
        BossSound.SetActive(true);
        DoorSound.SetActive(true);
        OtherDoors.SetActive(true);
        Destroy(CoatTrigger);

        var cameraPos = MainCamera.transform.position;
        MainCamera.GetComponent<PlayerCameraMovement>().IsFreezed = true;
        MainCamera.transform.position = new Vector3(-4.5f, -13, cameraPos.z);
        yield return new WaitForSeconds(1f);

        var spawnPos = BigDoor.transform.position + new Vector3(0, 0.4f);
        Destroy(BigDoor); // + мб звук ломания (или открытия) этой двери

        yield return new WaitForSeconds(1f);

        Instantiate(BossZombie, spawnPos, Quaternion.identity, GetComponentsInParent<Transform>()[1]);
        yield return new WaitForSeconds(2);

        MainCamera.GetComponent<PlayerCameraMovement>().IsFreezed = false;

        FindObjectOfType<PlayerAnimation>().IsFreezed = false;
        FindObjectOfType<PlayerMovement>().IsFreezed = false;
        FindObjectOfType<ShootingControl>().IsFreezed = false;
        FindObjectOfType<InventoryManagement>().IsFreezed = false;

        CallPanel.SetActive(true);
        isShowingBoss = false;
    }

    public void Stop()
    {
        NotificationText.gameObject.SetActive(false);
        HotkeyF.SetActive(false);
    }
}
