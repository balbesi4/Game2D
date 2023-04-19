using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShootingControl : MonoBehaviour
{
    public GameObject Bullet;
    private float bulletSpeed = 8f;

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.LeftButton))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        var crosshairPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //var rotateZ = Mathf.Atan2(crosshairPos.y, crosshairPos.x) * Mathf.Rad2Deg;
        var goal = new Vector3(crosshairPos.x, crosshairPos.y, 0);
        var direction = (goal - transform.position).normalized;
        var rotateSprite = GetComponent<GunContoller>().FlipNeeded;

        var bullet = Instantiate(Bullet, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        bullet.GetComponent<BulletControl>().RotateSprite(rotateSprite);
    }
}

public enum MouseButton
{
    LeftButton, MiddleButton, RightButton
}