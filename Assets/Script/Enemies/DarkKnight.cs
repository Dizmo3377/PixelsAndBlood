using System.Collections;
using UnityEngine;
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

    private bool reachedPlayer = false;

    private void Awake()
    {
        StartCoroutine(HitAndMoveOverTime(0.5f));
        CalculatePathToPlayer();
    }

    private void FixedUpdate()
    {
        if (sight.player == null) return;

        RotateFaceTo(sight.playerPos);
        if (canMove && HasBuiltPathToPlayer()) Move(pathPoints[CurrentPathIndex]);
    }

    private bool CanHitPlayer()
    {
        if (sight.player == null) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, sight.playerPos - transform.position);
        if (hit.collider == null || !hit.collider.CompareTag("Player")) return false;

        Vector2 distanceDelta = sight.playerPos - transform.position;
        if (distanceDelta.magnitude > minimalHitDelta) return false;

        return true;
    }

    private IEnumerator WaitForReachingPlayer() 
    { 
        yield return new WaitUntil(() => reachedPlayer); 
        reachedPlayer = false; 
    }

    private IEnumerator Hit(int combo)
    {
        int currentHit = 0;
        while (currentHit != combo)
        {
            if (CanHitPlayer())
            {
                Effects.instance.Slash(slashSpawnPoint);
                animator.SetTrigger("Hit");
                SoundManager.instance.Play("darkknight_attack");
                Player.instance.GetDamage(damage);
                yield return new WaitForSeconds(hitSpeed);
            }
            else yield return null;

            currentHit++;
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
        reachedPlayer = true;
    }

    private void BackDash(Vector3 target)
    {
        moveDir = target - transform.position;
        if (!CanHitPlayer()) return;

        rb.AddForce(-moveDir.normalized * dashForce * 100, ForceMode2D.Impulse);
        GameObject dust = ParticleManager.instance.Create("Dust", transform.position - new Vector3(0, 0.5f, 0));
        SoundManager.instance.Play("darkknight_dash");
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
            if (sight.seePlayer) BackDash(sight.playerPos);

            yield return new WaitForSeconds(delayBetweenAttacks);
        }
    }
}