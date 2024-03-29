using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private Image image;

    public int x, y;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Create(RoomType type)
    {
        image.sprite = UI.instance.roomIcons[(int)type];
    }

    public void SetState(bool state)
    {
        var color = image.color;
        color.a = state ? 0.58f : 0;
        image.color = color;
    }
}
