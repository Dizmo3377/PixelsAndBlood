using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionAutomat : InteractObject
{
    [SerializeField] private GameObject[] potions;
    [SerializeField] Transform spawnPoint;
    private int itemCost { get => LevelData.instance.lvl * 20; }
    private bool wasUsed = false;

    protected override void OnInteract() => Drop();

    private void Drop()
    {
        if (wasUsed || Player.instance.coins < itemCost) return;

        wasUsed = true;
        button.SetActive(false);
        Player.instance.coins -= itemCost;
        int randomPotion = Random.Range(0, potions.Length);

        Instantiate(potions[randomPotion], spawnPoint.position, Quaternion.identity);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasUsed || !collision.TryGetComponent(out Player entity)) return;
        player = entity;
        button.SetActive(true);
    }
}
