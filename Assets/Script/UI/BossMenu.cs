using UnityEngine;
using UnityEngine.UI;

public class BossMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text bossName;
    [SerializeField] private BossHealthBar healthBar;

    public void SetState(bool state, Boss boss = null)
    {
        canvas.gameObject.SetActive(state);
        bossName.gameObject.SetActive(state);
        bossName.text = state ? boss.showedName : " ";
        healthBar.Initialize(boss);
    }
}