using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject spillPrefab;
    [SerializeField] private GameObject smokePrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Bullet")) return;

        GameObject prefab = Instantiate(spillPrefab, transform.position, Quaternion.identity);
        Instantiate(smokePrefab, transform.position, Quaternion.identity).transform.parent = prefab.transform;
        Destroy(gameObject);
    }
}
