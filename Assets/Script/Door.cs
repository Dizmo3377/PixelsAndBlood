using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator anim;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>(); 
        anim = GetComponent<Animator>();
    }
    void OnEnable()
    {
        EnemyRoom.OnEntered += Switch;
    }

    void OnDisable()
    {
        EnemyRoom.OnEntered -= Switch;
    }

    void OnDestroy()
    {
        EnemyRoom.OnEntered -= Switch;
    }

    private void Switch()
    {
        if (boxCollider.isTrigger) { boxCollider.isTrigger = false; anim.SetTrigger("Close"); }
        else { boxCollider.isTrigger = true; anim.SetTrigger("Open"); }
    }
}
