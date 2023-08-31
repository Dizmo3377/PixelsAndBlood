using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : Enemy
{
    [SerializeField] private AnimationCurve curve;
    private float moveStartTime = 0f;
    private float speed = 0f;
    [SerializeField] private float moveDelay;
    private bool isAtacking = false;

    private void Start()
    {
        StartCoroutine(Move(moveDelay));
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = (rb.velocity * speed) * 1.2f;
            speed = curve.Evaluate(Time.time - moveStartTime);
            if (speed <= 0)
            {
                isAtacking = false;
                canMove = false;
                speed = 0f;
            }
        }
    }

    private IEnumerator Move(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (sight.seePlayer)
            {
                canMove = true;
                isAtacking = true;
                speed = 1f;
                moveStartTime = Time.time;
                rb.velocity = (sight.player.transform.position - this.transform.position).normalized;
                if (rb.velocity.x < 0) { spriteRenderer.flipX = false; }
                if (rb.velocity.x >= 0) { spriteRenderer.flipX = true; }

                animator.SetTrigger("Jump");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player.instance.GetDamge(2);
            isAtacking = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player") || !isAtacking) return;

        Player.instance.GetDamge(2);
        isAtacking = false;
    }
}