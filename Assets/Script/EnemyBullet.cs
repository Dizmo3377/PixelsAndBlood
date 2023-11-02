using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player entity)) entity.GetDamage(damage);
        Destroy(gameObject);
    }
}