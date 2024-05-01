using System.Collections;
using UnityEngine;

public class SCAR : Weapon
{
    [field:SerializeField] private float burstDelay { get; set; }
    public override void Shoot(Vector3 target) 
        => Player.instance.StartCoroutine(BurstShoot(0, 3, target));

    private IEnumerator BurstShoot(int currentBullet, int buletsAmount, Vector3 target)
    {
        if (currentBullet == buletsAmount) yield break;

        base.Shoot(target);
        SoundManager.instance.Play("scar");
        yield return new WaitForSeconds(burstDelay);
        Player.instance.StartCoroutine(BurstShoot(currentBullet + 1, buletsAmount, target));
    }
}