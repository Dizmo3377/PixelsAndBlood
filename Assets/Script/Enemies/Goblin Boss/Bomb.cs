using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float idleDelay;
    [SerializeField] private float burnDelay;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float blastWave;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(idleDelay);

        animator.SetTrigger("Burn");
        yield return new WaitForSeconds(burnDelay);

        animator.SetTrigger("Explode");
        Collider2D[] objectsInExplosionZone = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D obj in objectsInExplosionZone)
        {
            if (obj.TryGetComponent(out IPhysicallyDamagable damagable))
                damagable.GetDamage(damage);

            if (obj.TryGetComponent(out IPushable pushable))
                pushable.pushVector = (obj.transform.position - transform.position).normalized * blastWave;
        }

        CameraShaker.Shake(0.1f, 0.5f, 2);
        SoundManager.instance.Play("bomb");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(gameObject);
    }
}