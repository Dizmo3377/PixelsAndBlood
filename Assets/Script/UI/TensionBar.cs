using UnityEngine;
using UnityEngine.UI;

public class TensionBar : MonoBehaviour
{
    [SerializeField] private Sprite cellFull;
    [SerializeField] private Sprite cellEmpty;
    [SerializeField] private Image[] cells;
    [SerializeField] private GameObject loadingBar;

    public void Activate(bool state)
    {
        if (!state) 
            foreach (Image cell in cells) cell.sprite = cellEmpty;

        loadingBar.SetActive(state);
    }
    public void Highlight(int highLighteStage)
    {
        if (highLighteStage < 0 || highLighteStage >= cells.Length) return;
        cells[highLighteStage].sprite = cellFull;
    }
}