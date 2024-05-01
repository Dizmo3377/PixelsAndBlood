using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boar : Enemy
{
    [SerializeField] private float runDealy;
    [SerializeField] private float pushForce;
    [SerializeField] private float runTime;
    [SerializeField] private LayerMask layersForBounce;

    private Vector2 moveDir;

    private void Start() => StartCoroutine(Move(0.5f));

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Player")) Attack(collision.collider);

        if ((layersForBounce.value & (1 << collision.gameObject.layer)) != 0) Bounce(collision);
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        rb.velocity = moveDir * speed;
        RotateFaceTo((Vector2)transform.position + moveDir);
    }

    private void Attack(Collider2D toAttack)
    {
        Player player = toAttack.GetComponent<Player>();

        player.GetDamage(damage);
        player.GetComponent<IPushable>().pushVector 
            = (player.transform.position - transform.position).normalized * pushForce;
    }

    private void SetTriggerState(bool state)
    {
        canMove = state;
        rb.velocity = Vector2.zero;
        animator.SetBool("Run", state);
    }

    private void Bounce(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector3 newVelocity = (Vector3.Reflect(-collision.relativeVelocity, contactPoint.normal));
        moveDir = newVelocity.normalized;
    }

    private IEnumerator Move(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            moveDir = (sight.playerPos - transform.position).normalized;
            SetTriggerState(true);
            SoundManager.instance.PlayRandomRange("boar", 1, 4);
            yield return new WaitForSeconds(runTime);

            SetTriggerState(false);
            yield return new WaitForSeconds(runDealy);
        }
    }
}
