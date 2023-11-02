using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractObject : MonoBehaviour
{
    private CircleCollider2D trigger;
    protected Player player;

    [SerializeField] private float triggerRange;
    [SerializeField] private KeyCode key;
    [SerializeField] protected GameObject button;

    private void Start() 
    {
        trigger = GetComponent<CircleCollider2D>();
        trigger.radius = triggerRange;
    }

    private void Update()
    {
        if (Input.GetKeyUp(key) && player != null) OnInteract();
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player entity)) return;
        player = entity;
        button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Player entity)) return;
        player = null;
        button.SetActive(false);
    }

    protected abstract void OnInteract();
}