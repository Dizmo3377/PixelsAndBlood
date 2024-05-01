using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private EnemyRoom enemyRoom;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private SpriteRenderer sprite;

    public int waveId;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        enemyRoom = GetComponentInParent<EnemyRoom>();

        SetAlpha(0);
        Initialize();
    }
    private void Initialize() => enemyRoom.AddSpawner(this, waveId);

    private void SetAlpha(float value)
    {
        var tempColor = sprite.color;
        tempColor.a = value;
        sprite.color = tempColor;
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
