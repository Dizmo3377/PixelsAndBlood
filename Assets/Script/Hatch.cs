using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : MonoBehaviour
{
    private Player player;
    [SerializeField] private new CircleCollider2D collider;
    [SerializeField] private float triggerRange;
    [SerializeField] private GameObject button;

    private void Start() => collider.radius = triggerRange;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && player != null) Transition();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player p))
        {
            player = p;
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player p))
        {
            player = null;
            button.SetActive(false);
        }
    }

    private void Transition() => SceneController.StartSceneTransition("Dungeon_1");
}