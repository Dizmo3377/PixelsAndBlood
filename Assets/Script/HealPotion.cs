using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealPotion : InteractObject
{
    [SerializeField] int healAmount;
    protected override void OnInteract() => Drink();
    private void Drink()
    {
        Player.instance.Heal(healAmount);
        GameObject particle = ParticleManager.Create("Heal", transform.position);
        SoundManager.Play("potion");
        particle.transform.parent = Player.instance.transform;
        Destroy(gameObject);
    }
}