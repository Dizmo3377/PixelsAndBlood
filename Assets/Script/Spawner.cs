using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Spawner : MonoBehaviour
{
    [SerializeField] public int waveId;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] public EnemyRoom enemyRoom;
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyRoom = GetComponentInParent<EnemyRoom>();

        SetAlpha(0);
        Initialize();
    }

    private void SetAlpha(float value)
    {
        var tempColor = sprite.color;
        tempColor.a = value;
        sprite.color = tempColor;
    }

    private void Initialize()
    {
        enemyRoom.enemiesCount[waveId]++;
        enemyRoom.spawners.Add(this);
    }

    public void Activate()
    {
        StartCoroutine(ActivateCor());

        IEnumerator ActivateCor()
        {
            SetAlpha(1);
            yield return new WaitForSeconds(1.5f);
            Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>().room = enemyRoom;
            Destroy(gameObject);
        }
    }
}
