using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, ISaveableJson, IDamagable
{
    [Header("Stats")]
    public int healPoints;
    public int manaPoints;
    public int shieldPoints;

    public int coins;
    public int poisoned = 0;
    public int fired = 0;

    [Header("Limits")]
    public int maxHealPoints;
    public int maxManaPoints;
    public int maxShieldPoints;

    [HideInInspector] public bool isDead = false;
    private static float hitTime;

    [NonSerialized, HideInInspector] public AttackRange attack;

    public static Vector3 position => instance.transform.position;

    public string saveName => "Player";

    [NonSerialized] public static Player instance;

    private void OnEnable() => SceneController.onTransitionStart += () => JsonHelper.Save(this);
    private void OnDisable() => SceneController.onTransitionStart -= () => JsonHelper.Save(this);

    private void Awake()
    {
        instance = GetComponent<Player>();
        attack = GetComponentInChildren<AttackRange>();

        (healPoints, manaPoints, shieldPoints) = (maxHealPoints, maxManaPoints, maxShieldPoints);

        StartCoroutine(ShieldRecovery(2f));
        StartCoroutine(EffectsIterator());
    }

    private void Start() => JsonHelper.LoadOnNextLevel(this);
    public void AddCoins(int count) => coins += count;
    public void Heal(int amount) => healPoints = Mathf.Clamp(healPoints += amount, 0, maxHealPoints);
    public void ChangeMana(int amount) => manaPoints = Mathf.Clamp(manaPoints += amount, 0, maxManaPoints);
    public void GetDamage(int amount)
    {
        hitTime = Time.time;
        PlayerController.animator.SetTrigger("GetDamage");

        if (shieldPoints > 0)
        {
            shieldPoints = Mathf.Clamp(shieldPoints -= amount, 0, maxShieldPoints);
            if (shieldPoints == 0) CameraShaker.Shake(0.1f, 0.5f, 2);
        }
        else
        {
            healPoints -= amount;
            CameraShaker.Shake(0.1f, 0.5f, 2);
            if (healPoints <= 0) Die();
        }
    }

    public void Deactivate()
    {
        isDead = true;
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<PlayerController>());
    }

    private void Die()
    {
        Deactivate();

        UI.instance.bossMenu.SetState(false);
        UI.instance.FadeOut(2f);
        PlayerController.animator.SetTrigger("Death");
        PlayerController.weaponSprite.sprite = null;
        (healPoints, manaPoints, shieldPoints) = (0,0,0);

        StopAllCoroutines();
        StartCoroutine(ShowDeathMenu());

        IEnumerator ShowDeathMenu()
        {
            yield return new WaitForSeconds(2f);
            UI.instance.deathMenu.Show();
        }
    }

    private IEnumerator EffectsIterator()
    {
        //In future we can make "fired" and "poisened" classes
        while (true)
        {
            EffectApply(ref fired);
            EffectApply(ref poisoned);
            yield return new WaitForSeconds(1);
        }
    }

    private void EffectApply(ref int effect)
    {
        if (effect > 0)
        {
            effect -= 1;
            GetDamage(1);
        }
    }

    private IEnumerator ShieldRecovery(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (shieldPoints < maxShieldPoints && (Time.time - hitTime) > delay) shieldPoints++;
        }
    }

    public string SaveJson() => JsonUtility.ToJson(this, true);
    public void LoadJson(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
        shieldPoints = maxShieldPoints;
        (fired, poisoned) = (0,0);
        Inventory.UpdateWeaponIcon();
    }
}