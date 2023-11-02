using System.Collections;
using System;
using System.Collections.Generic;
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

    private Player playerData;
    [HideInInspector] public static UI instance;

    public void Awake()
    {
        instance = this;
        playerData = FindObjectOfType<Player>();
        UpdateWeapon();
    }

    private void Update()
    {
        coinsText.text = playerData.coins.ToString();
        healthText.text = playerData.healPoints.ToString();
        shieldText.text = playerData.shieldPoints.ToString();
        manaText.text = playerData.manaPoints.ToString();

        healthImage.fillAmount = (float)playerData.healPoints / playerData.maxHealPoints;
        shieldImage.fillAmount = (float)playerData.shieldPoints / playerData.maxShieldPoints;
        manaImage.fillAmount = (float)playerData.manaPoints / playerData.maxManaPoints;
    }

    public void ChangeWeapon() => Inventory.ChangeWeapon();
    public void UpdateWeapon()
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

    public void FadeOut(float duration) => group.DOFade(0, duration);
}