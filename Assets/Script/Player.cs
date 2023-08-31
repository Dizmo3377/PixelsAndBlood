using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
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


    private static float hitTime;
    [SerializeField] public PlayerController controller;

    public static Vector3 position => instance.transform.position;
    public static Player instance;
    public bool canShoot = true;
    [SerializeField] public AttackRange attack;

    private void Start()
    {
        instance = GetComponent<Player>();
        controller = GetComponent<PlayerController>();

        healPoints = maxHealPoints;
        manaPoints = maxManaPoints;
        shieldPoints = maxShieldPoints;

        StartCoroutine(ShieldRecovery(2f));
        StartCoroutine(EffectsIterator());
    }

    public void AddCoins(int count) => coins += count;

    public void GetDamge(int amount)
    {
        hitTime = Time.time;
        PlayerController.animator.SetTrigger("GetDamage");
        if (shieldPoints > 0)
        {
            shieldPoints -= amount;
            shieldPoints = Mathf.Clamp(shieldPoints, 0, maxShieldPoints);
        }
        else
        {
            healPoints -= amount;
            if (healPoints <= 0) Debug.Log("DEATH");
        }
    }

    private IEnumerator EffectsIterator()
    {
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
            GetDamge(1);
        }
    }

    public void Heal(int amount)
    {
        healPoints += amount;
        healPoints = Mathf.Clamp(healPoints, 0, maxHealPoints);
        GameObject particle = ParticleManager.Create("Heal", position);
        particle.transform.parent = instance.transform;
    }

    public void ChangeMana(int amount)
    {
        manaPoints += amount;
        manaPoints = Mathf.Clamp(manaPoints, 0, maxManaPoints);
    }

    private IEnumerator ShieldRecovery(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (shieldPoints < maxShieldPoints && (Time.time - hitTime) > delay)
                shieldPoints++;
        }
    }
}