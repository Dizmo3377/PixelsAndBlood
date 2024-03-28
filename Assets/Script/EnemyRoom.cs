using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoom : Room
{
    private bool isCleared = false;

    public int wave = 0;
    public int[] enemiesCount = new int[3];

    public delegate void EnterAction();
    public static event EnterAction OnEntered;
    public List<Spawner> spawners;

    [SerializeField] private GameObject chest;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCleared || !collision.CompareTag("Player")) return;

        OnEntered();
        PathfindingManager.instance.SetSurface(transform.position);
        ActivateSpawners(0);
    }

    public void OnEnemyKilled()
    {
        enemiesCount[wave] -= 1;

        if (KilledAllEnemies())
        {
            OnEntered();
            isCleared = true;
            SpawnChest();
            return;
        }

        if (enemiesCount[wave] < 1) ActivateSpawners(++wave);
    }

    private void SpawnChest()
    {
        //We spawn chest with only 50% chance 
        if(Random.Range(0, 2) == 1) return;

        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        float x,y;

        do
        {
            x = bounds.center.x + Random.Range(-bounds.extents.x, bounds.extents.x);
            y = bounds.center.y + Random.Range(-bounds.extents.y, bounds.extents.y);
        } 
        while (Physics2D.OverlapBoxAll(new Vector2(x, y), new Vector2(1, 1), 0f).Length > 0);

        Instantiate(chest, new Vector2(x,y), Quaternion.identity);
        ParticleManager.Create("ChestSpawn", new Vector2(x, y));
    }

    private bool KilledAllEnemies() => enemiesCount.All(x => x <= 0);
    private void ActivateSpawners(int wave) => spawners.Where(s => s.waveId == wave).ToList().ForEach(s => s.Activate());
}