using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [Header("Components")]
    [SerializeField] protected Sight sight;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [HideInInspector] protected bool canMove = false;

    [SerializeField] private int manaCount;

    [HideInInspector] public EnemyRoom room;

    public bool isDead { get; private set; } = false;
    [field:SerializeField] public int hp {  get; private set; }

    public void Heal(int amount) => hp += amount;

    public void GetDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;
        Effects.instance.DamageNumber(transform, amount);

        if (hp <= 0) Die();
        else animator.SetTrigger("GetDamage");
    }

    protected void RotateFaceTo(Vector3 point)
    {
        float lookDir = (point - transform.position).normalized.x;
        spriteRenderer.flipX = lookDir >= 0 ? false : true;
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        Effects.instance.SplashMana(transform, manaCount, 5);
        if (room != null) room.OnEnemyKilled();
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        StopAllCoroutines();

        canMove = false;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        sight.gameObject.SetActive(false);

        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
    }
}