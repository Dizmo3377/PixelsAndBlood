using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public static bool isPaused { get; private set; }
    public void SetPause(bool state)
    {
        if (Player.instance.isDead) return;

        canvas.gameObject.SetActive(state);
        Time.timeScale = (isPaused = state) ? 0 : 1;
        AudioListener.pause = state;
    }

    public void OnLeave() => Player.instance.Deactivate();
}