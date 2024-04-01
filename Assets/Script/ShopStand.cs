using UnityEngine;
using UnityEngine.UI;

public class ShopStand : InteractObject
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private GameObject saleItemsData;
    [field:SerializeField] private Text priceText;

    private SaleItem[] allSaleItems;
    private SaleItem objectOnSale;
    private int itemCost => objectOnSale.price * LevelData.instance.lvl;
    private bool wasUsed = false;

    private void Awake() => SetRandomOnSale();

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (wasUsed || !collision.TryGetComponent(out Player entity)) return;
        player = entity;
        button.SetActive(true);
    }

    protected override void OnInteract() => Buy();

    private void Buy()
    {
        if (wasUsed || Player.instance.coins < itemCost) return;

        wasUsed = true;
        button.SetActive(false);
        Player.instance.coins -= itemCost;
        itemSprite.sprite = null;
        SoundManager.Play("shop");

        Instantiate(objectOnSale.item, spawnPoint.position, Quaternion.identity);
    }

    private SaleItem GetRandomItem()
    {
        int randId, chance;

        do
        {
            randId = Random.Range(0, allSaleItems.Length);
            chance = Random.Range(1, 101);
        }
        while (allSaleItems[randId].dropChance < chance);

        return allSaleItems[randId];
    }

    private void SetRandomOnSale()
    {
        allSaleItems = saleItemsData.GetComponents<SaleItem>();

        objectOnSale = GetRandomItem();
        itemSprite.sprite = objectOnSale.item.GetComponent<SpriteRenderer>().sprite;
        priceText.text = itemCost.ToString();
    }
}