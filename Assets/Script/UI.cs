using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private Player playerData;

    private void Start() => playerData = FindObjectOfType<Player>();
    private void Update()
    {
        coinsText.text = playerData.coins.ToString();
        healthText.text = playerData.healPoints.ToString();
        shieldText.text = playerData.shieldPoints.ToString();
        manaText.text = playerData.manaPoints.ToString();

        healthImage.fillAmount = (float)playerData.healPoints / playerData.maxHealPoints;
        shieldImage.fillAmount = (float)playerData.shieldPoints / playerData.maxShieldPoints;
        manaImage.fillAmount = (float)playerData.manaPoints / playerData.maxManaPoints;

        Weapon currentWeapon = Inventory.slots[Inventory.currentWeapon];

        if (currentWeapon != null)
        {
            weaponImage.gameObject.SetActive(true);
            weaponImage.sprite = currentWeapon.icon;
            manaCostText.text = currentWeapon.manaCost.ToString();
        }
        else 
        { 
            weaponImage.gameObject.SetActive(false);
            manaCostText.text = "0";
        }
    }
}