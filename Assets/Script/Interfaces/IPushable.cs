using UnityEngine;
using UnityEngine.Rendering;

public interface IPushable
{
    public UnityEngine.Vector2 pushVector {  get; set; }
    public float pushTime { get; set; }
}