using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text shieldText;
    [SerializeField] private Text manaText;

    [SerializeField] private Image healthImage;
    [SerializeField] private Image shieldImage;
    [SerializeField] private Image manaImage;
    [SerializeField] private Image weaponImage;
    [SerializeField] private Text manaCostText;

    [SerializeField] private CanvasGroup group;
    [SerializeField] public DeathMenu deathMenu;
    [SerializeField] public PauseMenu pauseMenu;
    [SerializeField] public BossMenu bossMenu;

    [SerializeField] public Sprite[] roomIcons;

    private Player playerData;
    [HideInInspector] public static UI instance;

    public void Awake()
    {
        instance = this;
        playerData = FindFirstObjectByType<Player>();
        UpdateWeaponIcon();
    }

    private void Update() => Render();

    public void ChangeWeapon() => Inventory.ChangeWeapon();

    public void FadeOut(float duration) => group.DOFade(0, duration);

    private void Render()
    {
        coinsText.text = playerData.coins.ToString();
        healthText.text = playerData.healPoints.ToString();
        shieldText.text = playerData.shieldPoints.ToString();
        manaText.text = playerData.manaPoints.ToString();

        healthImage.fillAmount = (float)playerData.healPoints / Player.maxHealPoints;
        shieldImage.fillAmount = (float)playerData.shieldPoints / Player.maxShieldPoints;
        manaImage.fillAmount = (float)playerData.manaPoints / Player.maxManaPoints;
    }

    public void UpdateWeaponIcon()
    {
        Weapon weapon = Inventory.slots[Inventory.currentWeapon];

        if (weapon == null)
        {
            weaponImage.gameObject.SetActive(false);
            manaCostText.text = "0";
            weaponImage.sprite = null;
        }
        else
        {
            weaponImage.gameObject.SetActive(true);
            manaCostText.text = weapon.manaCost.ToString();
            weaponImage.sprite = weapon.icon;
        }
    }

}