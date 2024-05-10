using UnityEngine;

public class EffectsBar : MonoBehaviour
{
    [SerializeField] private GameObject fireSprite; 
    [SerializeField] private GameObject poisonSprite;
    private IEffectDamagable effectDamagable;

    private void Awake() => effectDamagable = GetComponentInParent<IEffectDamagable>();

    private void Update()
    {
        fireSprite.SetActive(effectDamagable.fired == 0 ? false : true);
        poisonSprite.SetActive(effectDamagable.poisoned == 0 ? false : true);
    }
}