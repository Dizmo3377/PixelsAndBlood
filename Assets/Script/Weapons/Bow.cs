using System.Collections;
using UnityEngine;

public class Bow : Weapon
{
    private float stretchTime = 0f;

    [field:SerializeField] private float stageTime { get; set; }
    [field: SerializeField] private int deafultDamage { get; set; }
    [field: SerializeField] private int maxStage { get; set; }

    private TensionBar tensionBar => Player.instance.tensionBar;

    private int CalculateDamage()
    {
        damage = deafultDamage;
        int stage = (int)(stretchTime / stageTime);
        stage = Mathf.Clamp(stage, 0, maxStage);

        SoundManager.instance.Play(stage > 2 ? "bow_delay" : "bow");

        return stage == 0 ? damage / 2 : damage * stage;
    }
    public override IEnumerator OnShoot()
    {
        float startTime = Time.time;
        tensionBar.Activate(true);

        while (Input.GetMouseButton(0))
        {
            int highLighteStage = (int)((Time.time - startTime) / stageTime) - 1;
            tensionBar.Highlight(highLighteStage);
            yield return null;
        }

        stretchTime = Time.time - startTime;
        damage = CalculateDamage();
    }

    public override void AfterShoot() 
    {
        base.AfterShoot();
        stretchTime = 0f;
        tensionBar.Activate(false);
    }
}