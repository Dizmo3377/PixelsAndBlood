using UnityEngine;

public class Ak47 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.instance.Play("ak47");
    }
}