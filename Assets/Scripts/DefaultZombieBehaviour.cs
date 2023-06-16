using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TerrainUtils;

public class DefaultZombieBehaviour : MonoBehaviour
{
    public bool IsShot;

    private Transform player;
    //private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed;
    private float pushSpeed;
    private float damage;
    private bool isTouchingPlayer;
    private NavMeshAgent agent;

    private void Awake()
    {
        damage = 10f;
        moveSpeed = 2f;
        pushSpeed = 1.2f;
        isTouchingPlayer = false;
        IsShot = false;
        player = FindObjectOfType<PlayerHealthManagement>().transform;
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void CalculateVelocity(Vector3 direction)
    {
        var speed = IsShot ? pushSpeed : moveSpeed;
        //rb.velocity = direction * speed;
    }

    private void Update()
    {
        //var targetDirection = (player.position - transform.position).normalized;
        //var direction = IsShot ? -targetDirection : targetDirection;
        //CalculateVelocity(direction);
        //ControlAnimations();

        if (!IsShot)
            agent.SetDestination(new Vector3(player.position.x, player.position.y, transform.position.z));
        else
            ReactOnDamage();
        ControlAnimations();
    }

    private void ReactOnDamage()
    {
        agent.enabled = false;
        var direction = (transform.position - player.position).normalized / 200;

        transform.position += direction;
        agent.enabled = true;
    }

    private void ControlAnimations()
    {
        var velocity = player.position - transform.position;

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