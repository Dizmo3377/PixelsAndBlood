using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK43 : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        PlayRandomSound();
    }

    private void PlayRandomSound() => SoundManager.Play($"mk43_{Random.Range(1, 3)}");
}