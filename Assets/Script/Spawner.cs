using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public int waveId;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] public EnemyRoom enemyRoom;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        var tempColor = sprite.color;
        tempColor.a = 0f;
        sprite.color = tempColor;  
        enemyRoom = transform.parent.GetComponent<EnemyRoom>();
        enemyRoom.enemiesCount[waveId]++;
        enemyRoom.spawners[waveId, enemyRoom.FindSpawner(waveId)] = this;
    }
    public void Activate()
    {
        StartCoroutine(ActivateCor());
    }

    private IEnumerator ActivateCor()
    {
        var tempColor = sprite.color;
        tempColor.a = 1f;
        sprite.color = tempColor;
        yield return new WaitForSeconds(1.5f);
        Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>().enemyRoom = this.enemyRoom;
        Destroy(gameObject);
    }
}
