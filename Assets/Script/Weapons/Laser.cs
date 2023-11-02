using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    public override void Shoot(Vector3 target)
    {
        Vector2 dir = (target - Player.position);
        dir.Normalize();
        Vector2 laserStart = Player.position + (Vector3)dir / 2;

        RaycastHit2D hit = Physics2D.Raycast(laserStart, dir, Mathf.Infinity, LayerMask.GetMask("Default"));

        Vector2 endpoint = hit.collider != null ? hit.point : laserStart * 1000f;

        Transform bullet = Instantiate(bulletPrefab, laserStart, Quaternion.identity).transform;
        bullet.GetComponent<LaserCast>().damage = damage;

        Vector3 rotateVector = endpoint - laserStart;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        float laserLength = Vector2.Distance(laserStart, endpoint) * 2.05f;
        bullet.localScale = new Vector3(bullet.localScale.x, laserLength, bullet.localScale.z);
    }
}