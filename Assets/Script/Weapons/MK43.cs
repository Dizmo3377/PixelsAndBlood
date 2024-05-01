using UnityEngine;

public class MK43 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.instance.PlayRandomRange("mk43_", 1, 2);
    }
}