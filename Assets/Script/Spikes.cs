using System.Collections;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private new BoxCollider2D collider;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private Sprite[] stateImages;
    [SerializeField] private float delay;

    private IEnumerator Start()
    {
        while (true)
        {
            for (int i = 0; i < 2; i++)
            {
                yield return new WaitForSeconds(delay);
                collider.enabled = !collider.enabled;
                render.sprite = stateImages[i];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Player.instance.GetDamge(1);
    }
}