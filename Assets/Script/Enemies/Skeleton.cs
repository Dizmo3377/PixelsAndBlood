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

    private GameObject arrow_cache;
    private Vector3 moveDir;

    private void Awake()
    {
        moveDir = transform.position;
        arrow_cache = arrow;
        arrow_cache.GetComponent<EnemyBullet>().damage = damage;

        StartCoroutine(ShootAndMoveOverTime(0.5f));
    }

    private void FixedUpdate()
    {
        if (sight.player != null) RotateFaceTo(sight.player.transform.position);
        if (transform.position != moveDir)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveDir, speed * Time.unscaledDeltaTime);
        }
    }
    private Vector2 Scatter(float amlitude) => new Vector3(Random.Range(-amlitude, amlitude), Random.Range(-amlitude, amlitude));
    private void MoveRandom() => moveDir = (Vector2)transform.position + Scatter(2f);

    public void ShootBurst(int buletsAmount)
    {
        //Closure 
        int currentBullet = 0;
        StartCoroutine(ShootBurst(buletsAmount));

        IEnumerator ShootBurst(int buletsAmount)
        {
            animator.SetTrigger("Idle");
            while (currentBullet != buletsAmount)
            {
                Shoot();
                yield return new WaitForSeconds(burstDelay);
                currentBullet++;
            }
            rb.velocity = Vector2.zero;
            MoveRandom();
            animator.SetTrigger("Run");
        }
    }

    private void Shoot()
    {
        if (sight.player == null) return;
        Vector3 target = sight.player.transform.position;
        Vector2 dir = (target - transform.position);
                dir.Normalize();
        Vector2 bulletSpawn = transform.position + (Vector3)dir;

        Rigidbody2D bullet = Instantiate(arrow_cache, bulletSpawn, Quaternion.identity).GetComponent<Rigidbody2D>();
        bullet.velocity = (dir + Scatter(accuracy)) * bulletForce;

        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator ShootAndMoveOverTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            if (!sight.seePlayer) { yield return null; continue; }

            ShootBurst(2);
            yield return new WaitForSeconds(shootDelay);
        }
    }
}