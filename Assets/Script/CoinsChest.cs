using UnityEngine;

public class CoinsChest : MonoBehaviour
{
    private bool opened = false;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] [Range(1f,20f)] private float coinVelocity;
    [SerializeField] private int coinsAmount;

    private void Open()
    {
        if (opened) return;

        opened = true;
        animator.SetTrigger("Open");
        SoundManager.PlayRandomRange("chest", 1, 3);

        for (int i = 0; i < coinsAmount; i++)
        {
            Rigidbody2D coin = Instantiate(coinPrefab.GetComponent<Rigidbody2D>(), transform.position, Quaternion.identity);
            coin.velocity = new Vector2(RandomVelocity(), RandomVelocity());
        }
    }

    private float RandomVelocity() => Random.Range(-coinVelocity, coinVelocity);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Open();
    }
}