using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock18 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        SoundManager.Play("glock18");
    }
}