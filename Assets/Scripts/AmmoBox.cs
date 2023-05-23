using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public Gun ThisGun;
    public int BulletCount;
    public string MessageUI;

    private void Start()
    {
        var stringGun = ThisGun is Gun.AK ? "AK-47" : "NONE";
        MessageUI = $"{BulletCount} патронов на {stringGun}";
    }
}
