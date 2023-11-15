using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [HideInInspector] public Boss boss;

    private int bossStartHealth;

    private void Update() => fill.fillAmount = boss == null ? 0 : (float)boss.hp / bossStartHealth;

    public void Initialize(Boss instance)
    {
        boss = instance;
        bossStartHealth = instance == null ? 1 : instance.hp;
    }
}