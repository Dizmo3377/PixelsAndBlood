using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Boss
{

    public override void Start() { base.Start(); StartCoroutine(Behavior()); }

    private IEnumerator Behavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(RunToPlayer());
            yield return StartCoroutine(Hit());
            yield return StartCoroutine(Heal());
            yield return new WaitForSeconds(10f);
        }
    }

    private IEnumerator Hit()
    {
        if (!CanHitPlayer(2f)) yield break;

        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / 2);
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

    private IEnumerator ThrowBomb()
    {
        yield return null;
    }

    //Run to player, rage and then punch
    private IEnumerator RunToPlayer()
    {
        Vector3 player = sight.player.transform.position;

        animator.SetBool("Run", true);
        RotateFaceTo(player);

        float startTime = Time.time;
        const float maxTime = 4f;

        while (Vector2.Distance(player, transform.position) > 1.5f && maxTime > Time.time - startTime)
        {
            transform.position = Vector3.MoveTowards(transform.position, player, speed * Time.unscaledDeltaTime);
            yield return null;
        }

        animator.SetBool("Run", false);
    }

    //Rage befor punch, so we give player some time to dodge
    private IEnumerator Rage()
    {
        yield return null;
    }

    private IEnumerator Heal()
    {
        animator.SetTrigger("Heal");
        GameObject particle = ParticleManager.Create("Heal", transform.position);
        particle.transform.parent = transform;
        yield return 1f;
    }
}