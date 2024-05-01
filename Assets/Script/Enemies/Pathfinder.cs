using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : Enemy
{
    private int _currentPathIndex = 0;
    protected int CurrentPathIndex
    {
        get {  return _currentPathIndex; }
        set 
        {
            _currentPathIndex = value;
            if (_currentPathIndex >= pathPoints.Length) DeletePath();
        } 
    }

    protected Vector3[] pathPoints { get; private set; }

    [SerializeField] private float pathUpdateSpeed;
    [SerializeField] private float reachingDelta;
    [SerializeField] private bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (!CanDrawGizmos()) return;

        Physics2D.Linecast(transform.position, pathPoints[CurrentPathIndex]);
        foreach (var item in pathPoints) Gizmos.DrawSphere(item, 0.3f);
    }

    private bool CanDrawGizmos() => drawGizmos && Application.isPlaying && HasBuiltPathToPlayer();

    protected IEnumerator GeneratePath()
    {
        if (sight.player == null) yield break;

        Path path = ABPath.Construct(transform.position, sight.playerPos);
        AstarPath.StartPath(path);
        yield return new WaitUntil(() => path.CompleteState != PathCompleteState.NotCalculated);

        pathPoints = path.vectorPath.ToArray();
        CurrentPathIndex = 0;
    }

    private IEnumerator UpdatePathToPlayer(float delay)
    {
        yield return StartCoroutine(GeneratePath());

        while (true)
        {
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(GeneratePath());
        }
    }

    protected bool ReachedCurrentPathPoint()
    {
        if (!HasBuiltPathToPlayer()) return true;

        Vector2 delta = pathPoints[CurrentPathIndex] - transform.position;
        return delta.magnitude <= reachingDelta;
    }

    protected bool HasBuiltPathToPlayer() => pathPoints != null && CurrentPathIndex < pathPoints.Length;
    protected void CalculatePathToPlayer() => StartCoroutine(UpdatePathToPlayer(pathUpdateSpeed));

    protected void DeletePath()
    {
        pathPoints = null;
        _currentPathIndex = 0;
    }
}