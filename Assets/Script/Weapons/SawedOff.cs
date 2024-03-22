using UnityEngine;

public class SawedOff : Weapon
{
    [SerializeField] private int fraction;
    public override void Shoot(Vector3 target)
    {
        for (int i = 0; i < fraction; i++) base.Shoot(target);

        PlayRandomSound();
    }

    private void PlayRandomSound() => SoundManager.Play($"sawedoff{Random.Range(1, 3)}");
}