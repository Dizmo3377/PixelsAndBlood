using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
using System;

public class DarkKnight : Pathfinder
{
    [SerializeField] private float minimalHitDelta;
    [SerializeField] private float delayBetweenAttacks;
    [SerializeField] private float hitSpeed;
    [SerializeField] private float dashForce;

    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private GameObject slashPrefab;

    private Vector3 moveDir;

    //In future replace this with single call (maybe)
    private static bool reachedPlayer = false;
    private Func<bool> hasReachedPlayer = new Func<bool>(() => reachedPlayer);

    private void Awake()
    {
        StartCoroutine(HitAndMoveOverTime(0.5f));
        CalculatePathToPlayer();
    }

    private void FixedUpdate()
    {
        if (sight.player == null) return;

        RotateFaceTo(sight.player.transform.position);
        if (canMove && HasBuiltPathToPlayer()) Move(pathPoints[CurrentPathIndex]);
    }

    private bool CanHitPlayer()
    {
        if (sight.player == null) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, sight.player.transform.position - transform.position);
        if (hit.collider == null || !hit.collider.CompareTag("Player")) return false;

        Vector2 distanceDelta = sight.player.transform.position - transform.position;
        if (distanceDelta.magnitude > minimalHitDelta) return false;

        return true;
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
        moveDir = target - transform.position;

        if (CanHitPlayer())
        {
            OnReachingPlayer();
            return;
        }

        rb.velocity = moveDir.normalized * speed;
        if (ReachedCurrentPathPoint()) CurrentPathIndex++;
    }

    private void OnReachingPlayer()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("Run", false);
        reachedPlayer = true;
        DeletePath();
        hasReachedPlayer.Invoke();
    }

    private void BackDash(Vector3 target)
    {
        moveDir = target - transform.position;
        if (!CanHitPlayer()) return;
        rb.AddForce(-moveDir.normalized * dashForce * 100, ForceMode2D.Impulse);
        GameObject dust = ParticleManager.Create("Dust", transform.position - new Vector3(0, 0.5f, 0));
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

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }
}