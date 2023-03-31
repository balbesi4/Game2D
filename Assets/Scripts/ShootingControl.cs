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
        var rotateZ = Mathf.Atan2(crosshairPos.y, crosshairPos.x) * Mathf.Rad2Deg;
        var goal = new Vector3(crosshairPos.x, crosshairPos.y, 0);
        var direction = (goal - transform.position).normalized;
        var bulletSprite = Bullet.GetComponent<SpriteRenderer>();

        if (rotateZ < 90f && rotateZ > -90f) bulletSprite.flipX = true;
        else bulletSprite.flipX = false;

        var bullet = Instantiate(Bullet, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}

public enum MouseButton
{
    LeftButton, MiddleButton, RightButton
}