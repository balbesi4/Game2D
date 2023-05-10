using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    }

    public Gun GetCurrentGun()
    {
        return guns[currentGun];
    }

    public void SetCurrentGun(int value)
    {
        if (value >= 0 && value < guns.Count)
            currentGun = value;
    }

    public void Add(Gun gun)
    {
        guns.Add(gun);
        gunsUI.Add(gun);
    }
}
