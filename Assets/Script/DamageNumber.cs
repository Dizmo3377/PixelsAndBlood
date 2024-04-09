using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private TMP_Text number;

    private void Awake() => number = GetComponent<TMP_Text>();
    public void SetNumber(int value) => number.text = value.ToString(); 

    public void SetRightScale()
    {
        if (transform.parent.localScale.x < 0) 
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void Tween()
    {
        DOTween.Sequence()
            .Append(number.DOFade(0f, 1f))
            .Join(transform.DOMoveY(transform.position.y + 0.4f, 0.1f))
            .OnComplete(() => Destroy(gameObject));
    }
}