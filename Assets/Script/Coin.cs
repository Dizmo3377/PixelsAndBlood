using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int coint = Random.Range(1, 4);
            Player.instance.AddCoins(coint);
            SoundManager.PlayRandomRange("coin", 1 , 2);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity * speed;
        speed = curve.Evaluate(Time.time - startTime);
    }
}
