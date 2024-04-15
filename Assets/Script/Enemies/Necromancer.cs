using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Enemy
{
    [SerializeField] private float burstDelay;
    [SerializeField] private float shootDelay;
    [SerializeField] private float bulletForce;
    [SerializeField] private float accuracy;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject enemyForSpawn;

    private List<Enemy> minions;
    private GameObject bullet_cache;
    private Vector3 moveDir;

    private void Awake()
    {
        moveDir = transform.position;
        bullet_cache = bullet;
        bullet_cache.GetComponent<EnemyBullet>().damage = damage;
        minions = new List<Enemy>();

        StartCoroutine(HitAndMoveOverTime(0.5f));
    }

    private void FixedUpdate()
    {
        if (sight.player != null) RotateFaceTo(sight.playerPos);
        if (transform.position != moveDir)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveDir, speed * Time.unscaledDeltaTime);
        }
    }

    private Vector2 Scatter(float amlitude) => new Vector3(Random.Range(-amlitude, amlitude), Random.Range(-amlitude, amlitude));

    private IEnumerator MoveRandom(int dashAmount)
    {
        animator.SetBool("Run", true);
        for (int i = 0; i < dashAmount; i++)
        {
            moveDir = (Vector2)transform.position + Scatter(3f);
            yield return new WaitForSeconds(0.4f);
        }
        moveDir = transform.position;
        animator.SetBool("Run", false);
    }

    private IEnumerator ShootBurst(int shotsAmount)
    {
        int currentShot = 0;
        while (currentShot != shotsAmount)
        {
            Shoot();
            animator.SetTrigger("Shoot");
            yield return new WaitForSeconds(burstDelay);
            currentShot++;
        }
    }

    private IEnumerator SpawnSlime(float delay)
    {
        if (!sight.seePlayer) yield break;

        Vector3 spawnPoint = sight.playerPos;
        animator.SetTrigger("Spawn");
        GameObject particle1 = ParticleManager.Create("NecromancerSpawn", transform.position - new Vector3(0,0.3f,0));
        GameObject particle2 = ParticleManager.Create("NecromancerSpawn", spawnPoint);
        particle1.transform.parent = transform;
        yield return new WaitForSeconds(delay);
        Enemy slime = Instantiate(enemyForSpawn, spawnPoint, Quaternion.identity).GetComponent<Enemy>();
        minions.Add(slime);
    }

    private void Shoot()
    {
        if (!sight.seePlayer) return;

        float angleStep = 20f;
        float currentAngle = -angleStep;

        SoundManager.Play("necromancer");

        for (int i = 0; i < 3; i++)
        {
            if (sight.player == null) continue;
            Vector3 target = sight.playerPos;
            Vector3 dir = Quaternion.Euler(0, 0, currentAngle) * (target - transform.position).normalized;
            Vector2 bulletSpawn = transform.position + dir * 1.2f;

            Rigidbody2D bullet = Instantiate(bullet_cache, bulletSpawn, Quaternion.identity).GetComponent<Rigidbody2D>();
            bullet.velocity = dir * bulletForce;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            currentAngle += angleStep;
        }
    }

    private IEnumerator HitAndMoveOverTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            for (int i = 0; i < 2; i++)
            {
                yield return StartCoroutine(MoveRandom(3));
                yield return StartCoroutine(ShootBurst(2));
                yield return new WaitForSeconds(shootDelay);
            }
            yield return StartCoroutine(SpawnSlime(0.5f));
        }
    }

    protected override void Die()
    {
        base.Die();
        foreach (Enemy minion in minions)
        {
            if (minion == null || minion.isDead) return;

            minion.GetDamage(100);
        }
    }
}