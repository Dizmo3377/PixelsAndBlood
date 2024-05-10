using UnityEngine;

public class Box : MonoBehaviour, IPhysicallyDamagable
{
    private int healPoints;
    private Room room;

    private void Awake()
    {
        room = GetComponentInParent<Room>();
        healPoints = Random.Range(2, 6);
    }

    public void GetDamage(int amount)
    {
        if (room != null && !room.canDestroyObjects) return;

        healPoints -= amount;

        ParticleManager.instance.Create("BoxDestroy", transform.position, 4);

        if (healPoints <= 0)
        {
            SoundManager.instance.PlayRandomRange("box_destroyed", 1, 4);
            Destroy(gameObject);
        }
    }
}