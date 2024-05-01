using UnityEngine;

public class PathfindingManager : Singletone<PathfindingManager>
{
    [SerializeField] private AstarPath path;
    public void SetSurfaceTo(Vector2 position) 
    {
        path.data.gridGraph.center = position;
        path.Scan();
    }
}