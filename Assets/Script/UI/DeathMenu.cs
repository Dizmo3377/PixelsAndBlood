using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public void Show() => canvas.gameObject.SetActive(true);
}