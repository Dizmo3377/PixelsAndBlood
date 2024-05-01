using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Extensions
{
    public static GameObject GetRandom(this GameObject[] arr) 
        => arr[Random.Range(0, arr.Length)];

    public static bool BoxOverlapsSomething(this Vector2 point, Vector2 size, float rotation)
       => Physics2D.OverlapBoxAll(point, size, rotation).Length > 0;

    public static bool CanReachPlayer(this Vector2 point)
    {
        RaycastHit2D hit = Physics2D.Raycast(point, (Vector2)Player.position - point);
        if (hit.collider == null || !hit.collider.CompareTag("Player")) return false;
        else return true;
    }
}