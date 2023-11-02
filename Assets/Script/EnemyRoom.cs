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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCleared || !collision.CompareTag("Player")) return;

        OnEntered();
        ActivateSpawners(0);
    }

    public void OnEnemyKilled()
    {
        enemiesCount[wave] -= 1;

        if (KilledAllEnemies())
        {
            OnEntered();
            isCleared = true;
            return;
        }

        if (enemiesCount[wave] < 1) ActivateSpawners(++wave);
    }

    private bool KilledAllEnemies() => enemiesCount.All(x => x <= 0);
    private void ActivateSpawners(int wave) => spawners.Where(s => s.waveId == wave).ToList().ForEach(s => s.Activate());
}