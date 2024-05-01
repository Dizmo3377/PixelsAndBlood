using UnityEngine;

public class HealPotion : InteractObject
{
    [SerializeField] int healAmount;
    protected override void OnInteract() => Drink();

    private void Drink()
    {
        Player.instance.Heal(healAmount);
        GameObject particle = ParticleManager.instance.Create("Heal", Player.instance.transform.position);
        SoundManager.instance.Play("potion");
        particle.transform.parent = Player.instance.transform;
        Destroy(gameObject);
    }
}