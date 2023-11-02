using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : Enemy
{
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDelay;

    private float moveStartTime = 0f;
    private float evaluation = 0f;
    private bool isAtacking = false;
    private Vector2 jumpDirection => (sight.player.transform.position - transform.position).normalized;
    private void Start() => StartCoroutine(Move(0.5f));
    private void OnCollisionEnter2D(Collision2D collision) => Attack(collision.collider);
    private void OnCollisionStay2D(Collision2D collision) => Attack(collision.collider);

    private void FixedUpdate()
    {
        if (!canMove || sight.player == null) return;

        evaluation = jumpCurve.Evaluate(Time.time - moveStartTime);
        rb.velocity = jumpDirection * evaluation * speed;
        if (evaluation <= 0) SetTriggerState(false);
    }

    private void Attack(Collider2D toAttack)
    {
        if (!toAttack.CompareTag("Player") || !isAtacking) return;

        Player.instance.GetDamage(damage);
        animator.SetTrigger("Attack");
        isAtacking = false;
    }

    private void SetTriggerState(bool state)
    {
        (canMove, isAtacking) = (state,state);
        evaluation = state ? 1f : 0f;
    }

    private IEnumerator Move(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            SetTriggerState(true);
            moveStartTime = Time.time;
            RotateFaceTo(sight.player.transform.position);
            animator.SetTrigger("Jump");
            yield return new WaitForSeconds(jumpDelay);
        }
    }
}