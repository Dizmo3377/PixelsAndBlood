using UnityEngine;

public class Glock18 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.instance.Play("glock18");
    }
}