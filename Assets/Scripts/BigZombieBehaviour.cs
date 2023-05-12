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
    private float moveSpeed, pushSpeed, damage;
    private float abilityDelay, lastAbilityTime;
    private bool isTouchingPlayer, isFreezed;

    private void Awake()
    {
        player = FindObjectOfType<PlayerAnimation>().GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        IsShot = false;
        isFreezed = false;
        abilityDelay = 6;
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
        yield return new WaitForSeconds(1f);
        var targetDirection = (player.position - transform.position).normalized;
        var poisonSpeed = 6f;

        var difference = player.position - transform.position;
        var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0f, 0f, rotateZ);

        var poison = Instantiate(Poison, transform.position, rotation, GetComponentsInParent<Transform>()[1]);
        poison.GetComponent<Rigidbody2D>().velocity = targetDirection * poisonSpeed;
        yield return new WaitForSeconds(0.2f);
        isFreezed = false;
    }

    private IEnumerator Boost()
    {
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
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
