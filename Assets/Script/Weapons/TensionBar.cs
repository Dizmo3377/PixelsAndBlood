using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TensionBar : MonoBehaviour
{
    public static TensionBar Instance;

    [SerializeField] private Sprite cellFull;
    [SerializeField] private Sprite cellEmpty;
    [SerializeField] private Image[] cells;
    [SerializeField] private GameObject loadingBar;

    private void Awake() => Instance = this;

    public void Activate(bool state)
    {
        if (!state) 
            foreach (Image cell in cells) cell.sprite = cellEmpty;

        loadingBar.SetActive(state);
    }
    public void Highlight(int number)
    {
        if (number < 0 || number >= cells.Length) return;
        cells[number].sprite = cellFull;
    }
}