using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public EnemyRoom room;

    [SerializeField] protected Sight sight;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [HideInInspector] protected bool canMove = false;

    [SerializeField] private int manaCount;
    [SerializeField] private int hp;
    public void GetDamage(int amount)
    {
        hp -= amount;

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
        Essence.SplashMana(transform, manaCount, 5);
        if (room != null) room.OnEnemyKilled();
        Destroy(gameObject);
    }
}