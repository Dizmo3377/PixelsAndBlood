using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    [SerializeField] GameObject gambleData;
    [SerializeField][Range(1f, 20f)] private float dropVelocity;

    private bool opened = false;
    private Animator animator;
    private GambleItem[] weapons;

    private void Start()
    {
        animator = GetComponent<Animator>();
        weapons = gambleData.GetComponents<GambleItem>();
    }

    private void Open()
    {
        if (opened) return;
        opened = true;

        animator.SetTrigger("Open");
        Rigidbody2D weaponRb = Instantiate(GetRandomItem().GetComponent<Rigidbody2D>(),
            transform.position, Quaternion.identity);
        weaponRb.velocity = new Vector2(RandomVelocity(), RandomVelocity());
    }

    private GameObject GetRandomItem()
    {
        int randId, chance;

        do
        {
            randId = Random.Range(0, weapons.Length);
            chance = Random.Range(1, 101);
        }
        while (weapons[randId].dropChance < chance);

        return weapons[randId].dropObject;
    }

    private float RandomVelocity() => Random.Range(-dropVelocity, dropVelocity);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) Open();
    }
}