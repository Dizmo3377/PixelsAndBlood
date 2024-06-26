using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : Pathfinder
{
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDelay;

    private float moveStartTime = 0f;
    private float evaluation = 0f;
    private bool canAttack = false;

    private Vector2 JumpDirection => (pathPoints[CurrentPathIndex] - transform.position).normalized;

    private void Start() => StartCoroutine(Move(0.5f));

    private void OnCollisionEnter2D(Collision2D collision) => Attack(collision.collider);
    private void OnCollisionStay2D(Collision2D collision) => Attack(collision.collider);

    private void FixedUpdate()
    {
        if (!CanJump()) return;

        if (ReachedCurrentPathPoint()) CurrentPathIndex++;

        evaluation = jumpCurve.Evaluate(Time.time - moveStartTime);

        if(pathPoints != null) rb.velocity = JumpDirection * evaluation * speed;
        if (evaluation <= 0) SetTriggerState(false);
    }

    private bool CanJump() => canMove && sight.seePlayer && HasBuiltPathToPlayer();

    private void Attack(Collider2D toAttack)
    {
        if (!toAttack.CompareTag("Player") || !canAttack) return;

        Player.instance.GetDamage(damage);
        animator.SetTrigger("Attack");
        canAttack = false;
    }

    private void SetTriggerState(bool state)
    {
        (canMove, canAttack) = (state, state);
        evaluation = state ? 1f : 0f;
    }

    private IEnumerator Move(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }
            DeletePath();

            yield return StartCoroutine(GeneratePath());
            SetTriggerState(true);
            moveStartTime = Time.time;

            if (sight.seePlayer) RotateFaceTo(sight.playerPos);
            animator.SetTrigger("Jump");
            SoundManager.instance.PlayRandomRange("slime_jump", 1, 3);
            yield return new WaitForSeconds(jumpDelay);
        }
    }
}