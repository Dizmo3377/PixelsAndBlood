using UnityEngine;

public class SawedOff : Weapon
{
    [field:SerializeField] public int fraction {  get; private set; }
    public override void Shoot(Vector3 target)
    {
        for (int i = 0; i < fraction; i++) base.Shoot(target);

        SoundManager.instance.PlayRandomRange("sawedoff", 1, 2);
    }
}