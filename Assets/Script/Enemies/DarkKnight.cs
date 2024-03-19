using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Pathfinding;
using System;

public class DarkKnight : Enemy
{
    //Minimal vector2 delta (player - enemy) when enemy can perform a hit
    [SerializeField] private float hitDistance;
    [SerializeField] private float hitDelay;
    [SerializeField] private float hitSpeed;
    [SerializeField] private float dashForce;

    [SerializeField] private Transform slashSpawnPoint;
    [SerializeField] private GameObject slashPrefab;

    private Vector3[] pathPoints;
    private int currentPathIndex = 0;

    private Vector3 dir;

    private static bool reachedPlayer = false;
    private Func<bool> hasReachedPlayer = new Func<bool>(() => reachedPlayer);

    private void Awake()
    {
        StartCoroutine(HitAndMoveOverTime(0.5f));
        StartCoroutine(UpdatePathToPlayer(0.2f));
    }

    private void FixedUpdate()
    {
        if (sight.player != null) RotateFaceTo(sight.player.transform.position);
        //Make it a function
        if (canMove && sight.player != null && pathPoints != null && currentPathIndex < pathPoints.Length)
        {
            Move(pathPoints[currentPathIndex]);
        }
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
        Vector3 closeDistanse = sight.player.transform.position - transform.position;
        if (closeDistanse.magnitude <= hitDistance)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("Run", false);
            reachedPlayer = true;
            pathPoints = null;
            currentPathIndex = 0;
            hasReachedPlayer.Invoke();
            return;
        }
        rb.velocity = dir.normalized * speed;

        Physics2D.Linecast(transform.position, target);

        // Check if reached the current path point
        if (dir.magnitude <= 0.2f)
        {
            currentPathIndex++;
        }
    }

    private void BackDash(Vector3 target)
    {
        dir = target - transform.position;
        if (dir.magnitude > hitDistance || !CanHitPlayer()) return;
        rb.AddForce(-dir.normalized * dashForce * 100, ForceMode2D.Impulse);
        GameObject dust = ParticleManager.Create("Dust", transform.position - new Vector3(0, 0.5f, 0));
        dust.transform.parent = transform;
    }

    private IEnumerator CalculatePathToPlayer()
    {
        if (sight.player == null) yield break;

        Path path = ABPath.Construct(transform.position, sight.player.transform.position);
        AstarPath.StartPath(path);
        yield return new WaitUntil(() => path.CompleteState != PathCompleteState.NotCalculated);

        pathPoints = path.vectorPath.ToArray();
        currentPathIndex = 0;
    }

    private void OnDrawGizmos()
    {
        if (pathPoints == null) return;
        foreach (var item in pathPoints)
        {
            Gizmos.DrawSphere(item, 0.3f);
        }
    }

    private IEnumerator HitAndMoveOverTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            animator.SetBool("Run", true);
            canMove = true;

            yield return StartCoroutine(CalculatePathToPlayer());

            yield return StartCoroutine(WaitForReachingPlayer());
            canMove = false;
            animator.SetBool("Run", false);
            yield return StartCoroutine(Hit(2));
            BackDash(sight.player.transform.position);

            yield return new WaitForSeconds(hitDelay);
        }
    }

    private IEnumerator UpdatePathToPlayer(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(CalculatePathToPlayer());
        }
    }
}