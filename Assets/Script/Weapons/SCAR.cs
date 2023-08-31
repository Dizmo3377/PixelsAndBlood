using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SCAR : Weapon
{
    [SerializeField] private float burstDelay;
    public override void Shoot(Vector3 target)
    {
        Console.WriteLine("OVERRIDE");
        Player.instance.StartCoroutine(BurstShoot(0,3,target));
    }

    private IEnumerator BurstShoot(int curBullet, int buletsAmount, Vector3 target)
    {
        if (curBullet == buletsAmount) yield break;

        base.Shoot(target);
        yield return new WaitForSeconds(burstDelay);
        Player.instance.StartCoroutine(BurstShoot(curBullet + 1,buletsAmount,target));
    }
}