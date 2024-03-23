using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWP : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.PlayRandomRange("awp", 1, 2);
    }
}