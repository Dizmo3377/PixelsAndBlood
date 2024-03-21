using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : Singletone<PathfindingManager>
{
    [SerializeField] private AstarPath path;
    public void SetSurface(Vector2 position) 
    {
        path.data.gridGraph.center = position;
        path.Scan();
    }
}