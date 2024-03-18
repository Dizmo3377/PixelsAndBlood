using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wallscript : MonoBehaviour
{
    [SerializeField] private CanvasGroup text;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float innerCircle;
    private Transform player;

    private void Start()
    {
        player = Player.instance.transform;
    }

    private void Update()
    {
        text.alpha = innerCircle + curve.Evaluate(Vector2.Distance(transform.position, player.position));
    }
}