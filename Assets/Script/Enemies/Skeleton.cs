using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    [SerializeField] private float burstDelay;
    [SerializeField] private float shootDelay;
    [SerializeField] private float bulletForce;
    [SerializeField] private float accuracy;
    [SerializeField] private GameObject arrow;

    private GameObject arrowCache;
    private Vector3 moveDir;

    private void Awake()
    {
        moveDir = transform.position;
        arrowCache = arrow; 
        arrowCache.GetComponent<EnemyBullet>().damage = damage;

        StartCoroutine(ShootAndMoveOverTime(0.5f));
    }

    private void FixedUpdate()
    {
        if (sight.seePlayer) RotateFaceTo(sight.playerPos);
        if (transform.position != moveDir && !isDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveDir, speed * Time.unscaledDeltaTime);
        }
        else animator.SetBool("Run", false);
    }
    private Vector2 RandomVector(float amlitude) => new Vector3(Random.Range(-amlitude, amlitude), Random.Range(-amlitude, amlitude));
    private void MoveRandom() => moveDir = (Vector2)transform.position + RandomVector(2f);

    public IEnumerator ShootBurst(int buletsAmount)
    {
        int currentBullet = 0;

        animator.SetTrigger("Idle");
        while (currentBullet != buletsAmount)
        {
            Shoot();
            yield return new WaitForSeconds(burstDelay);
            currentBullet++;
        }
        rb.velocity = Vector2.zero;
        animator.SetBool("Run", true);
        MoveRandom();
    }

    private void Shoot()
    {
        if (sight.player == null) return;
        Vector3 target = sight.playerPos;
        Vector2 dir = (target - transform.position);
                dir.Normalize();
        Vector2 bulletSpawn = transform.position + (Vector3)dir;

        Rigidbody2D bullet = Instantiate(arrowCache, bulletSpawn, Quaternion.identity).GetComponent<Rigidbody2D>();
        bullet.velocity = (dir + RandomVector(accuracy)) * bulletForce;

        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        SoundManager.instance.Play("bow");
    }

    private IEnumerator ShootAndMoveOverTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            yield return StartCoroutine(ShootBurst(2));
            yield return new WaitForSeconds(shootDelay);
        }
    }
}