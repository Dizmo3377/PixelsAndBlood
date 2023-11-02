using UnityEngine;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    private void Start()
    {
        DOTween.Sequence()
            .Append(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.6f))
            .AppendInterval(1f)
            .SetLoops(-1)
            .SetUpdate(true);

        DOTween.Sequence()
            .Append(transform.DORotate(new Vector3(0,0,-2), 2))
            .Append(transform.DORotate(new Vector3(0,0, 2), 2))
            .SetLoops(-1)
            .SetUpdate(true); 
    }
}