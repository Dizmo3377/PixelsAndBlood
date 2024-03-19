using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Bounds bounds;
    private void Start() => bounds = GetComponent<Collider2D>().bounds;
    private void OnDestroy()
    {
        if (AstarPath.active != null) AstarPath.active.UpdateGraphs(bounds);
    }
}