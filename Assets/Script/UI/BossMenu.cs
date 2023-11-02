using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMenu : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text bossName;

    public void SetState(bool state, Boss boss)
    {
        canvas.gameObject.SetActive(state);
        bossName.text = state ? boss.showedName : " ";
    }
}