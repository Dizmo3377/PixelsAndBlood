using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPhysicallyDamagable, IEffectDamagable
{
    private const int maxFireTicks = 10;
    private const int maxPoisonTicks = 10;

    [Header("Components")]
    [SerializeField] protected Sight sight;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] private int manaCount;

    [HideInInspector] public EnemyRoom room;
    [HideInInspector] protected bool canMove = false;

    private int _poisoned;
    private int _fired;

    public bool isDead { get; private set; } = false;
    [field:SerializeField] public int hp {  get; private set; }

    public int poisoned
    {
        get { return _poisoned; }
        set { _poisoned = Math.Clamp(value, 0, maxPoisonTicks); }
    }

    public int fired
    {
        get { return _fired; }
        set { _fired = Math.Clamp(value, 0, maxFireTicks); }
    }

    protected virtual void Awake() 
        => StartCoroutine(GetComponent<IEffectDamagable>().DebuffDamageIterator());

    public void Heal(int amount) => hp += amount;

    public void GetDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;
        Effects.instance.DamageNumber(transform, amount);

        if (hp <= 0) Die();
        else animator.SetTrigger("GetDamage");
    }

    public void ApplyFireDamage()
    {
        if (fired <= 0) return;

        fired--;
        GetDamage(1);
    }

    public void ApplyPoisonDamage()
    {
        if (poisoned <= 0) return;

        poisoned--;
        GetDamage(1);
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
        (fired, poisoned) = (0, 0);

        rb.velocity = Vector2.zero;
        animator.SetTrigger("Death");
    }
}