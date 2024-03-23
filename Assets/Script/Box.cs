using Pathfinding;
using UnityEngine;

public class Box : MonoBehaviour, IDamagable
{
    private int hp;
    private void Awake() => hp = Random.Range(2,6);
    public void GetDamage(int amount)
    {
        hp -= amount;

        ParticleManager.Create("BoxDestroy", transform.position, 4);

        if (hp <= 0)
        {
            SoundManager.PlayRandomRange("box_destroyed", 1, 4);
            Destroy(gameObject);
        }
    }
}