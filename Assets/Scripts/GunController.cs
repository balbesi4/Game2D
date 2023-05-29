using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GunController : MonoBehaviour
{
    private List<Gun> guns;
    private int currentGun;
    private GunsUI gunsUI;

    private void Awake()
    {
        gunsUI = FindObjectOfType<GunsUI>();
        guns = new() { Gun.Pistol };
        currentGun = 0;

        if (SceneManager.GetActiveScene().buildIndex == (int)Scene.Laboratory)
            guns.Add(Gun.AK);
    }

    public Gun GetCurrentGun()
    {
        return guns[currentGun];
    }

    public void SetCurrentGun(int value)
    {
        if (value >= 0 && value < guns.Count)
            currentGun = value;
        GetComponent<BulletCount>().ThisGun = (Gun)currentGun;
    }

    public bool Contains(Gun gun)
    {
        return guns.Contains(gun);
    }

    public void Add(Gun gun)
    {
        guns.Add(gun);
        gunsUI.Add(gun);
    }
}
