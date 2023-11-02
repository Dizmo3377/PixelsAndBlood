using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DarkKnight : Enemy
{
    //Minimal vector2 delta (player - enemy) when enemy can perform a hit
    [SerializeField] private float hitDistance;
    [SerializeField] private float hitDelay;
    [SerializeField] private float hitSpeed;
    [SerializeField] private float dashForce;

    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private GameObject slashPrefab;

    private Vector3 dir;

    private static bool reachedPlayer = false;
    private Func<bool> hasReachedPlayer = new Func<bool>(() => reachedPlayer);

    private void Awake() => StartCoroutine(HitAndMoveOverTime(0.5f));

    private void FixedUpdate()
    {
        //In future i should add pathfinding here
        if (sight.player != null) RotateFaceTo(sight.player.transform.position);
        if (canMove && sight.player != null) Move(sight.player.transform.position);
    }

    private bool CanHitPlayer()
    {
        if (sight.player == null) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, sight.player.transform.position - transform.position);
        if (hit.collider != null) return hit.collider.CompareTag("Player") ? true : false;

        return false;
    }

    private IEnumerator WaitForReachingPlayer() { yield return new WaitUntil(hasReachedPlayer); reachedPlayer = false; }
    private IEnumerator Hit(int combo)
    {
        int currentHit = 0;
        while (currentHit != combo)
        {
            if (CanHitPlayer())
            {
                Slash slash = Instantiate(slashPrefab, slashSpawnPoint).GetComponent<Slash>();
                animator.SetTrigger("Hit");
                slash.Play(sight.player.transform.position);
                Player.instance.GetDamage(damage);
            }
            currentHit++;
            yield return new WaitForSeconds(hitSpeed);
        }
    }

    private void Move(Vector3 target)
    {
        dir = target - transform.position;
        if (dir.magnitude <= hitDistance)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Run", false);
            reachedPlayer = true;
            hasReachedPlayer.Invoke();
            return;
        }
        rb.velocity = dir.normalized * speed;
    }

    private void BackDash(Vector3 target)
    {
        dir = target - transform.position;
        if (dir.magnitude > hitDistance || !CanHitPlayer()) return;
        rb.AddForce(-dir.normalized * dashForce * 100, ForceMode2D.Impulse);
        GameObject dust = ParticleManager.Create("Dust", transform.position - new Vector3(0,0.5f,0));
        dust.transform.parent = transform;
    }

    private IEnumerator HitAndMoveOverTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            animator.SetBool("Run", true);
            canMove = true;

            yield return StartCoroutine(WaitForReachingPlayer());
            canMove = false;
            animator.SetBool("Run", false);
            yield return StartCoroutine(Hit(2));
            BackDash(sight.player.transform.position);

            yield return new WaitForSeconds(hitDelay);
        }
    }
}