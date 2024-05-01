using UnityEngine;

public class WeaponRotater : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Sight sight;
    [SerializeField] private float maxOffsetDistance;
    [SerializeField] private float rotationOffset;
    [SerializeField] private Vector3 positionOffset;

    void Update()
    {
        if (!sight.seePlayer) return;

        Vector3 playerDelta = sight.playerPos - transform.position;
        Vector3 offset = Vector3.ClampMagnitude(playerDelta + positionOffset, maxOffsetDistance);

        spriteTransform.position = transform.position + offset;

        float angle = Mathf.Atan2(playerDelta.y, playerDelta.x) * Mathf.Rad2Deg;
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}
