using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ManaPotion : InteractObject
{
    [SerializeField] int manaAmount;
    protected override void OnInteract() => Drink();

    private void Drink()
    {
        Player.instance.ChangeMana(manaAmount);
        GameObject particle = ParticleManager.Create("Mana", Player.instance.transform.position);
        particle.transform.parent = Player.instance.transform;
        Destroy(gameObject);
    }
}