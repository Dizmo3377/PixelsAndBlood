using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandChanger : MonoBehaviour
{
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private Sight sight;

    private float positive;
    private float negative;

    private void Start()
    {
        positive = spriteTransform.transform.localPosition.x;
        negative = -spriteTransform.transform.localPosition.x;
    }

    void Update()
    {
        if (sight.player == null) return;

        Vector3 pos = spriteTransform.transform.localPosition;
        spriteTransform.localPosition = new Vector3(
            sight.player.transform.position.x > transform.position.x ? positive : negative,
            pos.y,
            pos.z);
    }
}