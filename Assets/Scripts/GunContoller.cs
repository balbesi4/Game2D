using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunContoller : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer gunSprite;

    public bool FlipNeeded { get; private set; }

    private void Start()
    {
        gunSprite = GetComponent<SpriteRenderer>();
        FlipNeeded = false;
    }
    
    private void Update()
    {
        Move();
        RotateGun();
    }

    private void Move()
    {
        var playerPos = player.transform.position;
        transform.position = playerPos;
    }

    private void RotateGun()
    {
        var diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var rotateZ = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;

        if (rotateZ < -90f || rotateZ > 90f)
        {
            gunSprite.flipX = true;
            FlipNeeded = false;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ - 180f);
        }
        else
        {
            gunSprite.flipX = false;
            FlipNeeded = true;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);
        }
    }
}
