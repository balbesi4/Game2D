using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigZombieBehaviour : MonoBehaviour
{
    public GameObject Poison;
    public bool IsShot;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveSpeed, pushSpeed, damage;
    private float abilityDelay, lastAbilityTime;
    private bool isTouchingPlayer, isFreezed;

    private void Awake()
    {
        player = FindObjectOfType<PlayerAnimation>().GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(1, 1);
        IsShot = false;
        isFreezed = false;
        abilityDelay = 4.5f;
        lastAbilityTime = 0;
        moveSpeed = 1.5f;
        pushSpeed = 0.7f;
        damage = 20;
    }

    public void CalculateVelocity(Vector3 direction, float speed)
    {
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        CheckAbility();
        if (!isFreezed)
        {
            var targetDirection = (player.position - transform.position).normalized;
            var direction = IsShot ? -targetDirection : targetDirection;
            var speed = IsShot ? pushSpeed : moveSpeed;
            CalculateVelocity(direction, speed);
        }
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

    private void CheckAbility()
    {
        if (!(lastAbilityTime < Time.time)) return;
        isFreezed = true;
        lastAbilityTime = Time.time + abilityDelay;
        var ability = Random.Range(1, 3);
        if (ability == 1)
            StartCoroutine(Boost());
        else
            StartCoroutine(Spit());
    }

    private IEnumerator Spit()
    {
        rb.velocity = Vector3.zero;
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(1f);
        animator.SetLayerWeight(1, 1);
        var targetDirection = player.position - transform.position;
        var poisonSpeed = 6f;

        var difference = player.position - transform.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, rotateZ);

        var offset = new Vector3(0, 0.5f);
        var position = transform.position + offset;
        var poison = Instantiate(Poison, position, rotation, GetComponentsInParent<Transform>()[1]);
        poison.GetComponent<Rigidbody2D>().velocity = (targetDirection - offset).normalized * poisonSpeed;
        yield return new WaitForSeconds(0.2f);
        isFreezed = false;
    }

    private IEnumerator Boost()
    {
        rb.velocity = Vector3.zero;
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(1f);
        animator.SetLayerWeight(1, 1);
        var targetDirection = (player.position - transform.position).normalized;
        var speed = moveSpeed * 2;
        CalculateVelocity(targetDirection, speed);
        damage = 30;
        yield return new WaitForSeconds(1f);
        isFreezed = false;
        damage = 20;
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
