using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake() => animator.enabled =  false;

    public void Play() => animator.enabled = true;
    public void OnFinish() => Destroy(gameObject);

    public void Rotate(Vector3 target)
    {
        transform.localPosition = Vector3.zero;
        Vector3 rotateVector = target - transform.position;
        float angle = Mathf.Atan2(rotateVector.y, rotateVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}