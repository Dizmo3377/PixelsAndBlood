using UnityEngine;

public class WeaponHandChanger : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Sight sight;

    private float xTurningRight;
    private float xTurningLeft;

    private void Awake()
    {
        xTurningRight = spriteTransform.transform.localPosition.x;
        xTurningLeft = -spriteTransform.transform.localPosition.x;
    }

    void Update()
    {
        if (!sight.seePlayer) return;

        Vector3 weaponPosition = spriteTransform.transform.localPosition;

        spriteTransform.localPosition = new Vector3
        (
            sight.playerPos.x > transform.position.x ? xTurningRight : xTurningLeft,
            weaponPosition.y,
            weaponPosition.z
        );
    }
}