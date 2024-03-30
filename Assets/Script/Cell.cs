using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject[] branches;

    public int x, y;

    public void SetIcon(RoomType type)
    {
        image.sprite = UI.instance.roomIcons[(int)type];
    }

    public void ChangeAlpha(float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void SetBranch(int id, bool state) => branches[id].SetActive(state);
}
