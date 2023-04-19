using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletControl : MonoBehaviour
{
    public float Damage { get; private set; }

    private void Start()
    {
        Damage = 50;
    }

    public void RotateSprite(bool isRotateNeeded)
    {
        if (isRotateNeeded) GetComponent<SpriteRenderer>().flipX = true;
        else GetComponent<SpriteRenderer>().flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gun") || collision.gameObject.CompareTag("Player")) return;

        if (collision.GetComponent<HealthManagement>() != null)
        {
            var health = collision.gameObject.GetComponent<HealthManagement>();
            health.DealDamage(Damage);
        }

        if (collision.gameObject.CompareTag("Door"))
            return;

        Destroy(gameObject);
    }
}
