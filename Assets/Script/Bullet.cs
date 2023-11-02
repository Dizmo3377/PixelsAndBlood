using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IDamagable entity) &&
            !collision.collider.CompareTag("Player")) entity.GetDamage(Inventory.primaryDamage);
        Destroy(gameObject);
    }
}