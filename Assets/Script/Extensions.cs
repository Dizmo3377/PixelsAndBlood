using UnityEngine;

public static class Extensions
{
    public static GameObject GetRandom(this GameObject[] arr) 
        => arr[Random.Range(0, arr.Length)];
}