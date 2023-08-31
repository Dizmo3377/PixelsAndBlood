using UnityEngine;
public class URL : MonoBehaviour
{
    public void Open(string url) => Application.OpenURL(url);
}