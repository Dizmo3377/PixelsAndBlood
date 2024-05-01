using UnityEngine;

public class MinimapTrigger : MonoBehaviour
{
    [SerializeField] private Room room;

    private void Awake()
    {
        if (room == null) room = GetComponentInParent<Room>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Minimap.HighlightCell(room.x, room.y, HighlightType.IsHere);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Minimap.HighlightCell(room.x, room.y, HighlightType.WasHere);
    }
}
