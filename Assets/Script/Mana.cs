using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1;
    [SerializeField] private AnimationCurve curve;
    private float startTime;
    private Transform player;
    private bool seePlayer;
    private void Start() => startTime = Time.time;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        int mana = Random.Range(1, 3);
        SoundManager.PlayRandomRange("essense", 1, 3, false);
        Player.instance.ChangeMana(mana);
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