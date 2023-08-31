using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float offset;
    [SerializeField] private float scrollSpeed;

    private void Awake() => material = GetComponent<Image>().material;

    private void FixedUpdate()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
