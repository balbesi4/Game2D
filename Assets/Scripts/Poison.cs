using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public float Damage { get; private set; }

    private void Awake()
    {
        Damage = 15;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie") || collision.gameObject.CompareTag("Wreck"))
            return;

        if (collision.GetComponent<PlayerHealthManagement>() != null)
        {
            var health = collision.gameObject.GetComponent<PlayerHealthManagement>();
            health.DealDamage(Damage);
        }

        Destroy(gameObject);
    }
}
