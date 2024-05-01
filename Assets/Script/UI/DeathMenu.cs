using System.Collections;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void Show() => canvas.gameObject.SetActive(true);

    public void ShowWithDelay(float delay)
    {
        StartCoroutine(ShowWithDelay());

        IEnumerator ShowWithDelay()
        {
            yield return new WaitForSeconds(delay);
            Show();
        }
    }
}