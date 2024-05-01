using UnityEngine;

public class Wallscript : MonoBehaviour
{
    [SerializeField] private float innerCircle;
    [SerializeField] private CanvasGroup text;
    [SerializeField] private AnimationCurve curve;

    private Transform player;

    private void Start() => player = FindFirstObjectByType<Player>().transform;

    private void Update()
    {
        text.alpha = innerCircle + curve.Evaluate(Vector2.Distance(transform.position, player.position));
    }
}