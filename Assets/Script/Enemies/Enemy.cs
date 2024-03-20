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
    [field:SerializeField] public int hp {  get; private set; }

    [HideInInspector] public EnemyRoom room;

    public void GetDamage(int amount)
    {
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

    protected virtual void Die()
    {
        Essence.SplashMana(transform, manaCount, 5);
        if (room != null) room.OnEnemyKilled();
        Destroy(gameObject);
    }
}