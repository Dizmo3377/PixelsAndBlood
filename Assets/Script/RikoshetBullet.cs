using UnityEngine;

public class RikoshetBullet : MonoBehaviour
{
    [SerializeField] private int bounceAmount;
    [SerializeField] private LayerMask layersForBounce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float minMagnitude;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out IPhysicallyDamagable entity)) 
            entity.GetDamage(Inventory.primaryDamage);

        if ((layersForBounce.value & (1 << collision.gameObject.layer)) != 0) 
        {
            Bounce(collision);
            bounceAmount--;
            if (bounceAmount <= 0) Destroy(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if(rb.velocity.magnitude < minMagnitude) Destroy(gameObject);
    }

    private void Bounce(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.contacts[0];
        Vector3 newVelocity = (Vector3.Reflect(-collision.relativeVelocity, contactPoint.normal));
        rb.velocity = newVelocity;
    }
}
