using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class DefaultZombieBehaviour : MonoBehaviour
{
    public Transform Player;
    public bool IsShot;

    private Rigidbody2D rb;
    private SpriteRenderer zombieSprite;
    private float moveSpeed;
    private float pushSpeed;
    private float damage;
    private bool isTouchingPlayer;

    private void Awake()
    {
        damage = 10f;
        moveSpeed = 1f;
        pushSpeed = 0.6f;
        isTouchingPlayer = false;
        IsShot = false;
        Player = FindObjectOfType<PlayerHealthManagement>().GetComponent<Transform>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        zombieSprite = GetComponent<SpriteRenderer>();
    }

    public void CalculateVelocity(Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        var targetDirection = Player.position - transform.position;
        var direction = IsShot ? -targetDirection : targetDirection;
        var speed = IsShot ? pushSpeed : moveSpeed;
        CalculateVelocity(direction, speed);
        ChangeSpriteOrientation(direction);
    }

    private void ChangeSpriteOrientation(Vector3 direction)
    {
        if (direction.x < 0 && !IsShot) zombieSprite.flipX = true;
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
