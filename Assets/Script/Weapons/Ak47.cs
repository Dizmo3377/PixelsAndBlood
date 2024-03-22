using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ak47 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.Play("ak47");
    }
}