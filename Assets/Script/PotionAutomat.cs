using TMPro;
using UnityEngine;

public class PotionAutomat : InteractObject
{
    [SerializeField] private GameObject[] potions;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text itemCostText;
    [SerializeField] private Animator animator;

    private bool wasUsed = false;
    private int itemCost => LevelData.instance.lvl * 20;

    private void Start() => itemCostText.text = itemCost.ToString();

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasUsed || !collision.TryGetComponent(out Player entity)) return;

        player = entity;
        button.SetActive(true);
    }

    protected override void OnInteract() => Drop();

    private void Drop()
    {
        if (wasUsed || Player.instance.coins < itemCost) return;

        wasUsed = true;
        button.SetActive(false);
        Player.instance.coins -= itemCost;
        int randomPotion = Random.Range(0, potions.Length);

        SoundManager.instance.Play("soda_automat");
        animator.SetTrigger("Drop");
        Instantiate(potions[randomPotion], spawnPoint.position, Quaternion.identity);
    }
}