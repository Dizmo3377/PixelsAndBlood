using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AWP : Weapon
{
    public override void Shoot(Vector3 target)
    {
        base.Shoot(target);
        PlayRandomSound();
    }

    private void PlayRandomSound() => SoundManager.Play($"awp{Random.Range(1, 3)}");
}