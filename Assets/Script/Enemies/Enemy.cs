using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected Sight sight;
    [SerializeField] protected Animator animator;
    [SerializeField] public EnemyRoom enemyRoom;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameObject manaPrefab;
    [SerializeField] private int manaCount;
    [HideInInspector] protected bool canMove = false;
    public int hp;
    public void GetDamage(int amount)
    {
        hp -= amount;

        if (hp <= 0) Die();
        else animator.SetTrigger("GetDamage");
    }

    private void Die()
    {
        SplashMana(manaCount, 5);
        enemyRoom.enemiesCount[enemyRoom.wave] -= 1;
        Destroy(this.gameObject);
    }

    private void SplashMana(int count, float force)
    {
        Rigidbody2D[] mana = new Rigidbody2D[count];
        for (int i = 0; i < mana.Length; i++)
        {
            mana[i] = Instantiate(manaPrefab.GetComponent<Rigidbody2D>(), transform.position, Quaternion.identity);
            mana[i].velocity = new Vector2(Random.Range(-force, force), Random.Range(-force, force));
        }
    }
}