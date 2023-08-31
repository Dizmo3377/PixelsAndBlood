using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    private bool isCleared = false;
    public int[] enemiesCount = new int[3];
    public int wave = 0;

    public delegate void EnterAction();
    public static event EnterAction OnEntered;

    public Spawner[,] spawners = new Spawner[3,20];

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCleared && collision.CompareTag("Player"))
        {
            OnEntered();
            ActivateSpawners(0);
        }
    }

    private void Update()
    {
        if (!isCleared)
        {
            if (enemiesCount[0] <= 0 && wave == 0)
            {
                wave = 1;
                ActivateSpawners(1);
            }
            else if (enemiesCount[1] <= 0 && wave == 1)
            {
                wave = 2;
                ActivateSpawners(2);
            }
            else if (enemiesCount[2] <= 0 && wave == 2)
            {
                wave = 3;
            }
            else if (wave == 3)
            {
                OnEntered();
                isCleared = true;
            }
        }
    }

    public int FindSpawner(int waveId)
    {
        for (int i = 0; i < spawners.GetLength(1); i++)
        {
            if (spawners[waveId, i] == null)
            {
                return i;
            }
        }
        return 0;
    }

    private void ActivateSpawners(int waveId)
    {
        for (int i = 0; i < spawners.GetLength(1); i++)
        {
            if (spawners[waveId, i] != null)
            {
                spawners[waveId, i].Activate();
            }
        }
    }
}
