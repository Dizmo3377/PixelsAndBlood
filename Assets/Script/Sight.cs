using UnityEngine;

public class Sight : MonoBehaviour
{
    [HideInInspector] public Player player;
    [HideInInspector] public bool seePlayer { get => player != null; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player = collision.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player = null;
    }
}