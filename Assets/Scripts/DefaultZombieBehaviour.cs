using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultZombieBehaviour : MonoBehaviour
{
    public Transform Player;
    private Rigidbody2D rb;
    private SpriteRenderer zombieSprite;
    private float moveSpeed;
    private float damage;
    private bool isTouchingPlayer;

    private void Awake()
    {
        damage = 10f;
        moveSpeed = 1f;
        isTouchingPlayer = false;
        Player = FindObjectOfType<PlayerHealthManagement>().GetComponent<Transform>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zombieSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var direction = Player.position - transform.position;
        rb.velocity = direction * moveSpeed;

        ChangeSpriteOrientation(direction);
    }

    private void ChangeSpriteOrientation(Vector3 direction)
    {
        if (direction.x < 0) zombieSprite.flipX = true;
        else zombieSprite.flipX = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealthManagement playerHealth))
        {
            isTouchingPlayer = true;
            StartCoroutine(DamageWhileTouching(playerHealth));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealthManagement playerHealth))
        {
            isTouchingPlayer = false;
        }
    }

    private IEnumerator DamageWhileTouching(PlayerHealthManagement playerHealth)
    {
        while (isTouchingPlayer)
        {
            playerHealth.DealDamage(damage);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
