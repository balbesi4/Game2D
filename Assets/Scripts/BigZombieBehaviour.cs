using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BigZombieBehaviour : MonoBehaviour
{
    public GameObject Poison;
    public bool IsShot;
    public AudioClip SpitSound;
    public AudioClip BoostSound;

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;
    private float damage;
    private float abilityDelay, lastAbilityTime;
    private bool isTouchingPlayer, isFreezed, isBoosted;

    private void Awake()
    {
        player = FindObjectOfType<PlayerAnimation>().GetComponent<Transform>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator.SetLayerWeight(1, 1);
        IsShot = false;
        isFreezed = false;
        isBoosted = false;
        abilityDelay = 4.5f;
        lastAbilityTime = 0;
        damage = 20;
    }

    private void Update()
    {
        CheckAbility();
        if (!isFreezed)
        {
            if (!IsShot)
                agent.SetDestination(new Vector3(player.position.x, player.position.y, transform.position.z));
            else
                ReactOnDamage();
        }
        ControlAnimations();
    }

    private void ReactOnDamage()
    {
        agent.enabled = false;
        var direction = (transform.position - player.position).normalized / 400;

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
        agent.SetDestination(transform.position);
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(1f);

        AudioSource.PlayClipAtPoint(SpitSound, Camera.main.transform.position);
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
        agent.SetDestination(transform.position);
        animator.SetLayerWeight(1, 0);
        yield return new WaitForSeconds(1f);

        AudioSource.PlayClipAtPoint(BoostSound, Camera.main.transform.position);
        isBoosted = true;
        animator.SetLayerWeight(1, 1);
        damage = 30;
        agent.SetDestination(player.position);
        agent.speed *= 4;
        yield return new WaitForSeconds(1.5f);

        agent.speed /= 4;
        isBoosted = false;
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
        else if (collision.gameObject.TryGetComponent(out DestroyControl destroyControl) && isBoosted)
        {
            destroyControl.DestroyItself();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DestroyControl destroyControl) && isBoosted)
        {
            destroyControl.DestroyItself();
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
