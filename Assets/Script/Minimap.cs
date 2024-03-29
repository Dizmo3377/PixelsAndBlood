using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    [SerializeField] private TMP_Text levelInfo;
    [SerializeField] private Cell[] cellsList;

    private static Cell[,] cells = new Cell[7,7];

    //[HideInInspector] public static Minimap instance;

    private void Awake()
    {
        PopulateCellsArray();
    }

    private void Start()
    {
        levelInfo.text = $"{LevelData.instance.stage}-{LevelData.instance.lvl}";
    }

    public static void CreateCell(int x, int y, RoomType type)
    {
        var cell = cells.Cast<Cell>().Where(c => c.x == x && c.y == y).FirstOrDefault();

        if (cell == null)
        {
            Debug.LogWarning("TY EBLAN");
            return;
        }

        cell.SetState(true);
        cell.Create(type);
    }

    private void PopulateCellsArray()
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                cells[x, y] = cellsList[y * cells.GetLength(1) + x];
                cells[x, y].x = x + 6;
                cells[x, y].y = cells.GetLength(0) - y + 5;
                cells[x, y].SetState(false);
            }
        }
    }
}
