using UnityEngine;

public abstract class Boss : Enemy
{
    [SerializeField] public string showedName;
    [SerializeField] private BossRoom bossRoom;

    public virtual void Start() => bossRoom = FindFirstObjectByType<BossRoom>();
    private void OnEnable() => BossRoom.OnEntered += () => UI.instance.bossMenu.SetState(true, this);
    private void OnDisable() => BossRoom.OnEntered += () => UI.instance.bossMenu.SetState(false, this);

    protected override void Die()
    {
        bossRoom.OnCleared();
        MusicManager.instance.StopMusic();
        base.Die();
    }
}