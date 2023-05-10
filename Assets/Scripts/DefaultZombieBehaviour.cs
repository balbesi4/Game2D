using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class DefaultZombieBehaviour : MonoBehaviour
{
    public Transform Player;
    public bool IsShot;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed;
    private float pushSpeed;
    private float damage;
    private bool isTouchingPlayer;

    private void Awake()
    {
        damage = 10f;
        moveSpeed = 2f;
        pushSpeed = 1.6f;
        isTouchingPlayer = false;
        IsShot = false;
        Player = FindObjectOfType<PlayerHealthManagement>().GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void CalculateVelocity(Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        var targetDirection = (Player.position - transform.position).normalized;
        var direction = IsShot ? -targetDirection : targetDirection;
        var speed = IsShot ? pushSpeed : moveSpeed;
        CalculateVelocity(direction, speed);
        ControlAnimations();
    }

    private void ControlAnimations()
    {
        var velocity = rb.velocity;

        if (velocity.y <= velocity.x && velocity.y >= -velocity.x)
            animator.SetInteger("Direction", (int)MoveDirection.Right);
        else if (velocity.y >= velocity.x && velocity.y <= -velocity.x)
            animator.SetInteger("Direction", (int)MoveDirection.Left);
        else if (velocity.y >= velocity.x && velocity.y >= -velocity.x)
            animator.SetInteger("Direction", (int)MoveDirection.Up);
        else
            animator.SetInteger("Direction", (int)MoveDirection.Down);
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