using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private float stageTime;
    [SerializeField] private int deafultDamage;
    [SerializeField] private int maxStage;
    private float stretchTime = 0f;

    private int CalculateDamage()
    {
        int stage = (int)(stretchTime / stageTime);
        stage = Mathf.Clamp(stage, 0, maxStage);

        if (stage > 2) SoundManager.Play("bow_delay");
        else SoundManager.Play("bow");

        if (stage == 0) return damage / 2;
        return damage * stage;
    }
    public override IEnumerator OnShoot()
    {
        float start = Time.time;
        TensionBar.Instance.Activate(true);

        while (Input.GetMouseButton(0)) 
        {
            TensionBar.Instance.Highlight((int) ( (Time.time - start) / stageTime) - 1);
            yield return null;
        }

        stretchTime = Time.time - start;
        damage = CalculateDamage();
    }

    public override void AfterShoot() 
    {
        base.AfterShoot();
        damage = deafultDamage;
        stretchTime = 0f;
        TensionBar.Instance.Activate(false);
    }
}