using UnityEngine;

public class Barrel : MonoBehaviour, IPhysicallyDamagable
{
    [SerializeField] private GameObject spillPrefab;
    [SerializeField] private GameObject smokePrefab;
    private Room room;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
    }

    public void GetDamage(int amount)
    {
        if (room != null && !room.canDestroyObjects) return;

        GameObject prefab = Instantiate(spillPrefab, transform.position, Quaternion.identity);
        SoundManager.instance.Play("barrel_explosion");
        Instantiate(smokePrefab, transform.position, Quaternion.identity).transform.parent = prefab.transform;
        Destroy(gameObject);
    }
}
