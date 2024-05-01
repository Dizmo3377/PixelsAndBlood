using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyRoom : Room
{
    private bool isCleared = false;
    private int wave = 0;
    private int[] enemiesCount = new int[3];

    [SerializeField] private GameObject chest;

    public delegate void EnterAction();
    public static event EnterAction OnEntered;
    public List<Spawner> spawners;

    private void Start() => canDestroyObjects = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCleared || !collision.CompareTag("Player")) return;

        OnEntered();
        canDestroyObjects = true;
        PathfindingManager.instance.SetSurfaceTo(transform.position);
        ActivateSpawners(0);
    }

    private bool KilledAllEnemies() => enemiesCount.All(x => x <= 0);
    private void ActivateSpawners(int wave) => spawners.Where(s => s.waveId == wave).ToList().ForEach(s => s.Activate());

    public void AddSpawner(Spawner spawner, int wave)
    {
        enemiesCount[wave]++;
        spawners.Add(spawner);
    }

    public void OnEnemyKilled()
    {
        enemiesCount[wave] -= 1;

        if (KilledAllEnemies())
        {
            OnEntered();
            OnCleared();
            return;
        }

        if (enemiesCount[wave] < 1) ActivateSpawners(++wave);
    }

    private void OnCleared()
    {
        isCleared = true;
        //Highlight new rooms on minimap
        for (int i = 0; i < branches.Length; i++)
        {
            if (branches[i].activeSelf)
            {
                var currentCell = Minimap.GetCell(x, y);
                currentCell.SetBranch(i, true);
                Vector2Int connectedRoom = new Vector2Int(x, y) + Level.IntToScalar(i);
                Minimap.HighlightCell(connectedRoom.x, connectedRoom.y, HighlightType.WasNotHere);

            }
        }
        SpawnChest();
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
        ParticleManager.instance.Create("ChestSpawn", new Vector2(x, y));
    }
}