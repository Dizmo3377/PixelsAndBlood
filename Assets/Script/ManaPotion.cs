using UnityEngine;

public class ManaPotion : InteractObject
{
    [SerializeField] private int manaAmount;

    protected override void OnInteract() => Drink();

    private void Drink()
    {
        Player.instance.manaPoints += manaAmount;
        GameObject particle = ParticleManager.instance.Create("Mana", Player.instance.transform.position);
        SoundManager.instance.Play("potion");
        particle.transform.parent = Player.instance.transform;
        Destroy(gameObject);
    }
}