using UnityEngine;

public class SaleItem : MonoBehaviour
{
    [Range(1, 100)] public int dropChance;
    public int price;
    public GameObject item;
}