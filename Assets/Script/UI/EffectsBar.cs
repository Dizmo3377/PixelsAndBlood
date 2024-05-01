using UnityEngine;

public class EffectsBar : MonoBehaviour
{
    [SerializeField] private GameObject fireSprite; 
    [SerializeField] private GameObject poisonSprite; 

    private void Update()
    {
        fireSprite.SetActive(Player.instance.fired == 0 ? false : true);
        poisonSprite.SetActive(Player.instance.poisoned == 0 ? false : true);
    }
}