using UnityEngine;

public class BossRoom : Room
{
    private bool isCleared;
    private bool activated = false;

    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject hatch;

    public delegate void EnterAction();
    public static event EnterAction OnEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated || isCleared || !collision.CompareTag("Player")) return;

        Activate();
        PathfindingManager.instance.SetSurfaceTo(transform.position);
        OnEntered();
    }

    private void Activate()
    {
        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        MusicManager.instance.PlayBossMusic();
        activated = true;
    }

    public void OnCleared()
    {
        isCleared = true;
        hatch.SetActive(true);
    }
}