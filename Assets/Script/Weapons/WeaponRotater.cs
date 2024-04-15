using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponRotater : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Sight sight;
    [SerializeField] private float maxOffsetDistance;
    [SerializeField] private float rotateOffset;
    [SerializeField] private Vector3 positionOffset;

    void Update()
    {
        if (sight.player == null) return;

        Vector3 player = sight.playerPos - transform.position;
        Vector3 offset = Vector3.ClampMagnitude(player + positionOffset, maxOffsetDistance);

        spriteTransform.position = transform.position + offset;

        float angle = Mathf.Atan2(player.y, player.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);
    }
}
