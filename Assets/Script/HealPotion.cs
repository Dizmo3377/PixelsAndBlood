using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPotion : MonoBehaviour
{
    [SerializeField] int healAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
