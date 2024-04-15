using UnityEngine;

public class Sight : MonoBehaviour
{
    [HideInInspector] public Player player;
    public Vector3 playerPos => player.transform.position;
    public bool seePlayer => player != null;
    private void OnTriggerEnter2D(Collider2D collision)
        => player = collision.CompareTag("Player") ? collision.GetComponent<Player>() : player;
    private void OnTriggerExit2D(Collider2D collision) 
        => player = collision.CompareTag("Player") ? null : player;
}