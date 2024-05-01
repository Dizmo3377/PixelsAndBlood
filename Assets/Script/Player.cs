using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour, ISaveableJson, IDamagable
{
    private const int maxCoins = 9999;
    private const int maxFireTicks = 10;
    private const int maxPoisonTicks = 10;
    private const float shieldRecoveryDelay = 2f;
    private const float debuffDamageDelay = 1f;

    public const int maxHealPoints = 10;
    public const int maxManaPoints = 200;
    public const int maxShieldPoints = 6;

    [SerializeField] private int _coins;
    [SerializeField] private int _healPoints;
    [SerializeField] private int _manaPoints;
    private int _shieldPoints;
    private int _poisoned;
    private int _fired;
    private float lastHitTime;

    [NonSerialized, HideInInspector] public static Player instance;
    [NonSerialized, HideInInspector] public AttackRange attack;
    [NonSerialized, HideInInspector] public TensionBar tensionBar;
    [NonSerialized, HideInInspector] public PlayerController controller;

    public string saveName => "Player";

    public static Vector3 position => instance.transform.position;

    [field:NonSerialized] public bool isDead {  get; private set; }

    public int healPoints
    {
        get { return _healPoints; }
        private set { _healPoints = Math.Clamp(value, 0, maxHealPoints); }
    }

    public int manaPoints 
    {
        get { return _manaPoints; }
        set { _manaPoints = Math.Clamp(value, 0, maxManaPoints); }
    }

    public int shieldPoints
    {
        get { return _shieldPoints; }
        set { _shieldPoints = Math.Clamp(value, 0, maxShieldPoints); }
    }

    public int coins
    {
        get { return _coins; }
        set { _coins = Math.Clamp(value, 0, maxCoins); }
    }

    public int poisoned
    {
        get { return _poisoned; }
        set { _poisoned = Math.Clamp(value, 0, maxPoisonTicks); }
    }

    public int fired
    {
        get { return _fired; }
        set { _fired = Math.Clamp(value, 0, maxFireTicks); }
    }

    private void OnEnable() => SceneController.onTransitionStart += () => JsonHelper.Save(this);
    private void OnDisable() => SceneController.onTransitionStart -= () => JsonHelper.Save(this);

    private void Awake()
    {
        instance = GetComponent<Player>();
        attack = GetComponentInChildren<AttackRange>();
        tensionBar = GetComponentInChildren<TensionBar>();
        controller = GetComponent<PlayerController>();
        controller.enabled = true;

        (healPoints, manaPoints, shieldPoints) = (maxHealPoints, maxManaPoints, maxShieldPoints);

        StartCoroutine(ShieldRecoveryIterator());
        StartCoroutine(DebuffDamageIterator());
    }

    private void Start() => JsonHelper.LoadOnNextLevel(this);

    private IEnumerator DebuffDamageIterator()
    {
        while (true)
        {
            ApplyFireDamage();
            ApplyPoisonDamage();
            yield return new WaitForSeconds(debuffDamageDelay);
        }
    }

    private IEnumerator ShieldRecoveryIterator()
    {
        while (true)
        {
            yield return new WaitForSeconds(shieldRecoveryDelay);
            if (shieldPoints < maxShieldPoints && (Time.time - lastHitTime) > shieldRecoveryDelay) shieldPoints++;
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return;

        healPoints += amount;
    }

    public void GetDamage(int amount)
    {
        if (amount < 0) return;

        lastHitTime = Time.time;
        controller.animator.SetTrigger("GetDamage");

        if (shieldPoints > 0)
        {
            shieldPoints -= amount;
            SoundManager.instance.Play("shield");
            if (shieldPoints == 0) CameraShaker.Shake(0.1f, 0.5f, 2);
        }
        else
        {
            healPoints -= amount;
            CameraShaker.Shake(0.1f, 0.5f, 2);
            SoundManager.instance.PlayRandomRange("hit", 1, 2);
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
        controller.weaponSprite.sprite = null;
        (healPoints, manaPoints, shieldPoints) = (0,0,0);

        Deactivate();
        StopAllCoroutines();

        UI.instance.bossMenu.SetState(false);
        UI.instance.FadeOut(2f);
        UI.instance.deathMenu.ShowWithDelay(2f);

        SoundManager.instance.Play("death");
        MusicManager.instance.StopMusic();

        controller.animator.SetTrigger("Death");
        controller.walkingSound.Stop();
    }

    private void ApplyFireDamage()
    {
        if (fired <= 0) return;

        fired--;
        GetDamage(1);
    }

    private void ApplyPoisonDamage()
    {
        if (poisoned <= 0) return;

        poisoned--;
        GetDamage(1);
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