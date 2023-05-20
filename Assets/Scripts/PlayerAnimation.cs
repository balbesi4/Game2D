using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public bool IsFreezed;

    private Animator animator;
    private Rigidbody2D rb;
    private const string stringDir = "Direction";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        IsFreezed = false;
    }

    private void Update()
    {
        if (IsFreezed)
        {
            animator.SetLayerWeight(1, 0);
            SetIdleDirection(transform.position + new Vector3(1, 0, 0));
        }
        else
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            if (horizontal != 0 || vertical != 0)
            {
                animator.SetLayerWeight(1, 1);
                SetMoveDirection(mousePos);
            }
            else
            {
                animator.SetLayerWeight(1, 0);
                SetIdleDirection(mousePos);
            }
        }
    }

    private void SetIdleDirection(Vector3 mousePos)
    {
        var direction = mousePos - transform.position;
        if (direction.y < direction.x && direction.y < -direction.x)
            animator.SetInteger(stringDir, (int)MoveDirection.Down);
        else if (direction.y <= direction.x && direction.y >= -direction.x)
            animator.SetInteger(stringDir, (int)MoveDirection.Right);
        else if (direction.y > direction.x && direction.y > -direction.x)
            animator.SetInteger(stringDir, (int)MoveDirection.Up);
        else
            animator.SetInteger(stringDir, (int)MoveDirection.Left);
    }

    private void SetMoveDirection(Vector3 mousePos)
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var direction = mousePos - transform.position;

        if (horizontal > 0)
        {
            if (direction.x > 0)
                animator.SetInteger(stringDir, (int)MoveDirection.Right);
            else
                animator.SetInteger(stringDir, (int)MoveDirection.BackLeft);
        }
        else if (horizontal < 0)
        {
            if (direction.x < 0)
                animator.SetInteger(stringDir, (int)MoveDirection.Left);
            else
                animator.SetInteger(stringDir, (int)MoveDirection.BackRight);
        }
        else if (vertical > 0)
        {
            if (direction.y > 0)
                animator.SetInteger(stringDir, (int)MoveDirection.Up);
            else
                animator.SetInteger(stringDir, (int)MoveDirection.BackDown);
        }
        else
        {
            if (direction.y < 0)
                animator.SetInteger(stringDir, (int)MoveDirection.Down);
            else
                animator.SetInteger(stringDir, (int)MoveDirection.BackUp);
        }
    }
}

public enum MoveDirection
{
    Up,
    Down,
    Right,
    Left,
    BackUp,
    BackDown,
    BackRight,
    BackLeft
}
