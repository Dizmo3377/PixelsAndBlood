using UnityEngine;

public class Mana : MonoBehaviour
{
    private float startTime;
    private Transform player;
    private bool seePlayer;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1;
    [SerializeField] private AnimationCurve curve;

    private void Start() => startTime = Time.time;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        int value = Random.Range(1, 3);
        SoundManager.instance.PlayRandomRange("essense", 1, 3, false);
        Player.instance.manaPoints += value;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity * speed;
        speed = curve.Evaluate(Time.time - startTime);

        if (seePlayer && Time.time - startTime >= 0.5f)
            rb.velocity = (player.position - transform.position).normalized * 20;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return; 

        player = collision.transform;
        seePlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        player = null;
        seePlayer = false;
    }
}