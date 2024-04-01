using System.Collections;
using UnityEngine;

public class Goblin : Boss
{
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float throwForce;

    private Vector2 spawnPoint;
    private AudioSource walkingSound;
    private int startHealth;

    public override void Start() 
    {
        base.Start();
        StartCoroutine(Behavior());

        spawnPoint = transform.position;
        startHealth = hp;
        walkingSound = GetComponent<AudioSource>();
        walkingSound.Play();
        walkingSound.Pause();
    }

    protected override void Die()
    {
        SoundManager.Play("goblin_death");
        base.Die();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        walkingSound.Stop();
    }

    private IEnumerator Behavior()
    {
        //Just a little delay befor start
        yield return new WaitForSeconds(1f);

        //Sequence of actions, boss pattern basically
        while (true)
        {
            //3 Hits in close combat
            for (int i = 0; i < 3; i++)
            {
                yield return StartCoroutine(RunToPlayer());
                yield return StartCoroutine(Hit());
                yield return new WaitForSeconds(1f);
            }

            //Run in to the middle of the room
            yield return StartCoroutine(RunToSpawnPoint());

            //Throw 3 bombs at player
            for (int i = 0; i < 3; i++)
            {
                ThrowBomb();
                yield return new WaitForSeconds(0.4f);
            }

            //Hill if hp is less then 70 percent
            if (hp <= startHealth * 0.7f) yield return StartCoroutine(Heal());

            //Spawn slimes if half health left
            if (hp <= startHealth * 0.5f) SpawnMinions();
        }
    }

    private IEnumerator Hit()
    {
        if (!CanHitPlayer(2f)) yield break;

        animator.SetTrigger("Hit");
        SoundManager.PlayRandomRange("bonk", 1, 3);
        yield return new WaitForSeconds(0.1f);
        sight.player.GetComponent<IDamagable>().GetDamage(damage);
    }

    private bool CanHitPlayer(float hitDistance)
    {
        if (sight.player == null) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, sight.player.transform.position - transform.position);

        if (hit.collider != null &&
            Vector2.Distance(transform.position, sight.player.transform.position) < hitDistance &&
            hit.collider.CompareTag("Player"))
            return true;

        return false;
    }

    private void ThrowBomb()
    {
        if (sight.player == null) return;
        RotateFaceTo(sight.player.transform.position);

        Vector3 throwVector = (sight.player.transform.position - transform.position).normalized;

        Rigidbody2D bomb = Instantiate
        (
            bombPrefab,
            transform.position + throwVector,
            Quaternion.identity
        ).GetComponent<Rigidbody2D>();

        bomb.AddForce(throwVector * throwForce * 100, ForceMode2D.Impulse);
        SoundManager.PlayRandomRange("woosh", 1, 3);
    }

    private IEnumerator RunToSpawnPoint()
    {
        animator.SetBool("Run", true);
        walkingSound.UnPause();
        RotateFaceTo(spawnPoint);

        float startTime = Time.time;
        const float maxTime = 4f;

        while (Vector2.Distance(spawnPoint, transform.position) > 1.5f && maxTime > Time.time - startTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint, speed * Time.unscaledDeltaTime);
            yield return null;
        }

        animator.SetBool("Run", false);
        walkingSound.Pause();
    }

    private IEnumerator RunToPlayer()
    {
        if (sight.player == null) yield break;
        Vector3 player = sight.player.transform.position;

        animator.SetBool("Run", true);
        walkingSound.UnPause();
        RotateFaceTo(player);

        float startTime = Time.time;
        const float maxTime = 4f;

        while (Vector2.Distance(player, transform.position) > 1.5f && maxTime > Time.time - startTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, player, speed * Time.unscaledDeltaTime);
            yield return null;
        }

        animator.SetBool("Run", false);
        walkingSound.Pause();
    }

    private void SpawnMinions()
    {
        SoundManager.Play("necromancer");
        Instantiate(minionPrefab, (Vector2)transform.position + Vector2.left * 2, Quaternion.identity);
        Instantiate(minionPrefab, (Vector2)transform.position + Vector2.right * 2, Quaternion.identity);
    }

    private IEnumerator Heal()
    {
        animator.SetTrigger("Heal");
        SoundManager.Play("goblin_heal");
        GameObject particle = ParticleManager.Create("Heal", transform.position);
        particle.transform.parent = transform;
        Heal(30);
        yield return new WaitForSeconds(2f);
    }
}