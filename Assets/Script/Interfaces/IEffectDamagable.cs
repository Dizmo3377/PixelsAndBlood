using System.Collections;
using UnityEngine;

public interface IEffectDamagable
{
    private const float debuffDamageDelay = 1f;

    public int fired { get; set; }
    public int poisoned { get; set; }

    public void ApplyFireDamage();
    public void ApplyPoisonDamage();

    public IEnumerator DebuffDamageIterator(float delay = debuffDamageDelay)
    {
        while (true)
        {
            ApplyFireDamage();
            ApplyPoisonDamage();
            yield return new WaitForSeconds(delay);
        }
    }
}