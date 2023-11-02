using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boar : Enemy
{
    [SerializeField] private float runDealy;
    [SerializeField] private float runTime;
    [SerializeField] private LayerMask layersForBounce;

    private Vector2 moveDir;

    private void Start() => StartCoroutine(Move(0.5f));
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.collider.CompareTag("Player")) Attack(collision.collider);

        //&& - logical "AND" works with bools
        //& - bitwise "AND" works on the bit level, compares every bit.
        //<< - bitwise left shift

        //NOTE! deafult layer is 0, so 1 << 0 = 1 (0001). Therefore 1 << collision.gameObject.layer
        //should offset 1 (0001) to match layersForBounce.value in binary system, for instance
        //001010000001 & 0001. There is one match (1 and 1), so value will not be 0, so there is
        //layer in layer mask :)
        if ((layersForBounce.value & (1 << collision.gameObject.layer)) != 0) Bounce(collision);
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        rb.velocity = moveDir * speed;
        RotateFaceTo((Vector2)transform.position + moveDir);
    }

    private void Attack(Collider2D toAttack) => Player.instance.GetDamage(damage);
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

            moveDir = (sight.player.transform.position - transform.position).normalized;
            SetTriggerState(true);
            yield return new WaitForSeconds(runTime);
            SetTriggerState(false);
            yield return new WaitForSeconds(runDealy);
        }
    }
}
