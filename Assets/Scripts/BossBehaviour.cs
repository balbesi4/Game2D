using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    private GameObject player;
    private Animator animator;
    private Rigidbody2D rb;
    private float speed = 1f;

    private void Start()
    {
        player = FindObjectOfType<PlayerHealthManagement>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (transform.position.x > -13)
            rb.velocity = new Vector3(-speed, 0, 0);
        else
            rb.velocity = (player.transform.position - transform.position).normalized * speed;

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
            playerHealth.DealDamage(100);
    }
}
