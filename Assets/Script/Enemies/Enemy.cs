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
    private bool isDead = false;

    [HideInInspector] public EnemyRoom room;

    [field:SerializeField] public int hp {  get; private set; }

    public void GetDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;

        if (hp <= 0) Die();
        else animator.SetTrigger("GetDamage");
    }

    public void Heal(int amount) => hp += amount;

    protected void RotateFaceTo(Vector3 point)
    {
        float lookDir = (point - transform.position).normalized.x;
        spriteRenderer.flipX = lookDir >= 0 ? false : true;
    }

    protected virtual void OnDeath()
    {
        StopAllCoroutines();
        canMove = false;
        sight.gameObject.SetActive(false);
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Death");
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        Essence.SplashMana(transform, manaCount, 5);
        if (room != null) room.OnEnemyKilled();
        OnDeath();
    }
}