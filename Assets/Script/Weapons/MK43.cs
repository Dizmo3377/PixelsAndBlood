using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK43 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.PlayRandomRange("mk43_", 1, 2);
    }
}