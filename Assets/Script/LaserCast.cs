using UnityEngine;

public class LaserCast : MonoBehaviour
{
    public int damage { get; set; }
    [field:SerializeField] public float decaySpeed {  get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamagable damagable)) damagable.GetDamage(damage);
    }

    private void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x - decaySpeed, scale.y, scale.z);

        if (scale.x <= 0) Destroy(gameObject);
    }
}