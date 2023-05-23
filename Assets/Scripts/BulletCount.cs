using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    private int[] bullets;

    public Gun ThisGun { get; set; }

    public int Count { get { return bullets[(int)ThisGun]; } }

    private void Start()
    {
        bullets = new[] { 0, 30 };
        ThisGun = Gun.Pistol;
    }

    public void Add(int newBullets, Gun gun)
    {
        var actualBullets = bullets[(int)gun] + newBullets;
        bullets[(int)gun] = actualBullets >= 0 ? actualBullets : 0;
        FindObjectOfType<GunsUI>().SetBulletCount(gun, actualBullets);
    }
}
