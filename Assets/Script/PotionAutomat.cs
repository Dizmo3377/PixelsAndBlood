using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionAutomat : InteractObject
{
    [SerializeField] private GameObject[] potions;
    [SerializeField] Transform spawnPoint;
    [SerializeField] TMP_Text itemCostText;
    [SerializeField] private Animator animator;

    private int itemCost { get => LevelData.instance.lvl * 20; }
    private bool wasUsed = false;

    protected override void Start()
    {
        base.Start();
        itemCostText.text = itemCost.ToString();
    }

    protected override void OnInteract() => Drop();

    private void Drop()
    {
        if (wasUsed || Player.instance.coins < itemCost) return;

        wasUsed = true;
        button.SetActive(false);
        Player.instance.coins -= itemCost;
        int randomPotion = Random.Range(0, potions.Length);

        SoundManager.Play("soda_automat");
        animator.SetTrigger("Drop");
        Instantiate(potions[randomPotion], spawnPoint.position, Quaternion.identity);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasUsed || !collision.TryGetComponent(out Player entity)) return;
        player = entity;
        button.SetActive(true);
    }
}
