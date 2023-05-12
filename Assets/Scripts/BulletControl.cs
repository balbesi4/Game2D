using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour
{
    public float Damage { get; set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gun")
            || collision.gameObject.CompareTag("Player")
            || collision.gameObject.CompareTag("Drop")
            || collision.gameObject.CompareTag("Door")) return;

        if (collision.GetComponent<HealthManagement>() != null)
        {
            var health = collision.gameObject.GetComponent<HealthManagement>();
            health.DealDamage(Damage);
        }

        Destroy(gameObject);
    }
}
