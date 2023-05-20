using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class ShootingControl : MonoBehaviour
{
    public GameObject Bullet;
    public bool IsFreezed;

    private Animator animator;
    private PlayerMovement playerMovement;
    private GunController gunController;
    private float bulletSpeed = 10f;
    private bool isShooting, isSpraying, isStopped;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gunController = GetComponent<GunController>();
        playerMovement = GetComponent<PlayerMovement>();
        isShooting = false;
        isSpraying = false;
        isStopped = false;
        IsFreezed = false;
    }

    private void Update()
    {
        if (IsFreezed) return;

        if (gunController.GetCurrentGun() is Gun.Pistol)
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftButton) && !isShooting)
                StartCoroutine(ShootPistol());
        }
        else if (gunController.GetCurrentGun() is Gun.AK)
        {
            if (Input.GetMouseButton((int)MouseButton.LeftButton))
            {
                if (!isShooting)
                    StartCoroutine(StopForSpraying());
                if (!isSpraying && !isStopped)
                    StartCoroutine(Spray());
            }
            else if (isShooting)
            {
                StartCoroutine(StopSpraying());
            }
        }
    }

    private IEnumerator StopSpraying()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetLayerWeight((int)Gun.AK + 2, 0);
        playerMovement.IsFreezed = false;
        isShooting = false;
    }

    private IEnumerator ShootPistol()
    {
        isShooting = true;
        playerMovement.IsFreezed = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

        var pistolDir = GetPistolDirection(mousePos - transform.position);
        animator.SetInteger("Pistol direction", pistolDir);
        animator.SetLayerWeight((int)Gun.Pistol + 2, 1);
        yield return new WaitForSeconds(0.2f);

        Shoot(pistolDir, Gun.Pistol, 40);
        yield return new WaitForSeconds(0.1f);
        animator.SetLayerWeight((int)Gun.Pistol + 2, 0);
        playerMovement.IsFreezed = false;
        isShooting = false;
    }

    private IEnumerator StopForSpraying()
    {
        isShooting = true;
        playerMovement.IsFreezed = true;
        isStopped = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        var pistolDir = GetPistolDirection(mousePos - transform.position);

        animator.SetInteger("Pistol direction", pistolDir);
        animator.SetLayerWeight((int)Gun.AK + 2, 1);
        yield return new WaitForSeconds(0.2f);
        isStopped = false;
    }

    private IEnumerator Spray()
    {
        isSpraying = true;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);
        var pistolDir = GetPistolDirection(mousePos - transform.position);
        animator.SetInteger("Pistol direction", pistolDir);
        Shoot(pistolDir, Gun.AK, 30);
        yield return new WaitForSeconds(0.15f);
        isSpraying = false;
    }

    private int GetPistolDirection(Vector3 direction)
    {
        if (direction.y >= direction.x && direction.y >= -direction.x)
            return 0;
        else if (direction.y <= direction.x && direction.y <= -direction.x)
            return 1;
        else if (direction.y <= direction.x && direction.y >= -direction.x)
            return 2;
        else
            return 3; 
    }

    private void Shoot(int pistolDirection, Gun gun, float damage)
    {
        var crosshairPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var goal = new Vector3(crosshairPos.x, crosshairPos.y, 0);
        var direction = goal - transform.position;

        var position = transform.position;
        var offset = new Vector3(0, 0);
        if (pistolDirection == 1)
        {
            offset = new Vector3(0, 0.1f);
            position += offset;
            Bullet.GetComponent<SpriteRenderer>().sortingLayerName = "Grabbable";
        }
        else if (pistolDirection == 2)
        {
            offset = gun == Gun.Pistol
                ? new Vector3(0.65f, 0.27f)
                : new Vector3(0.65f, 0.2f);
            position += offset;
            Bullet.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        }
        else if (pistolDirection == 3)
        {
            offset = gun == Gun.Pistol
                ? new Vector3(-0.65f, 0.27f)
                : new Vector3(-0.65f, 0.2f);
            position += offset;
            Bullet.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        }
        else
        {
            Bullet.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
        }

        var difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, rotateZ);
        var bullet = Instantiate(Bullet, position, rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = (direction - offset).normalized * bulletSpeed;
        bullet.GetComponent<BulletControl>().Damage = damage;
    }
}

public enum MouseButton
{
    LeftButton, MiddleButton, RightButton
}

public enum Gun
{
    Pistol,
    AK
}