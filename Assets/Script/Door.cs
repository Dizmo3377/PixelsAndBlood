using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator animator;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>(); 
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        EnemyRoom.OnEntered += Switch;
        BossRoom.OnEntered += Switch;
    }

    void OnDisable() 
    {
        EnemyRoom.OnEntered -= Switch;
        BossRoom.OnEntered -= Switch;
    }

    private void Switch()
    {
        boxCollider.isTrigger = !boxCollider.isTrigger;
        animator.SetTrigger(boxCollider.isTrigger ? "Open" : "Close");
    }
}
