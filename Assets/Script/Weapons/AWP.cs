using UnityEngine;

public class AWP : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.instance.PlayRandomRange("awp", 1, 2);
    }
}