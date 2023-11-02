using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private static Transform cameraHolder;
    private void Awake() => cameraHolder = Camera.main.transform.parent;
    public static void Shake(float duration, float strength, int  vibrato) => cameraHolder.DOShakePosition(duration, strength, vibrato);
}