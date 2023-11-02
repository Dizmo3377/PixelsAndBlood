using UnityEngine;

public class Barrel : MonoBehaviour, IDamagable
{
    [SerializeField] private GameObject spillPrefab;
    [SerializeField] private GameObject smokePrefab;

    public void GetDamage(int amount)
    {
        GameObject prefab = Instantiate(spillPrefab, transform.position, Quaternion.identity);
        Instantiate(smokePrefab, transform.position, Quaternion.identity).transform.parent = prefab.transform;
        Destroy(gameObject);
    }
}
