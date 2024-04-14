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

    private void Awake()
    {
        PopulateCellsArray();
    }

    private void Start()
    {
        if (LevelData.instance.stage > 1) gameObject.SetActive(false);
    }

    private void Update()
    {
        levelInfo.text = $"{LevelData.instance.stage}-{LevelData.instance.lvl}";
        if (Application.isEditor && Input.GetKeyDown(KeyCode.X)) Regenerate();
    }

    public static void InitializeCell(int x, int y, RoomType type, bool changeAlpha = true)
    {
        var cell = GetCell(x,y);

        if(changeAlpha) cell.ChangeAlpha(HighlightTypeToFloat(HighlightType.WasNotHere));
        cell.SetIcon(type);
    }

    public static Cell GetCell(int x, int y)
    {
        var cell = cells.Cast<Cell>().Where(c => c.x == x && c.y == y).FirstOrDefault();

        if (cell == null)
        {
            Debug.LogWarning($"Cell not found or out of boundaries {x} - {y}");
            return null;
        }

        return cell;
    }

    public static void HighlightCell(int x, int y, HighlightType type)
    {
        var cell = GetCell(x, y);
        float alpha = HighlightTypeToFloat(type);
        if(cell != null) cell.ChangeAlpha(alpha);
    }

    public static float HighlightTypeToFloat(HighlightType type)
    {
        switch (type)
        {
            case HighlightType.Hidden: return 0f;
            case HighlightType.WasNotHere: return 0.2f;
            case HighlightType.WasHere: return 0.6f;
            case HighlightType.IsHere: return 1f;
            default: return 0f;
        }
    }

    private void PopulateCellsArray()
    {
        for (int y = 0; y < cells.GetLength(0); y++)
        {
            for (int x = 0; x < cells.GetLength(1); x++)
            {
                cells[x, y] = cellsList[y * cells.GetLength(1) + x];
                cells[x, y].x = x;
                cells[x, y].y = cells.GetLength(0) - y - 1;
                cells[x, y].ChangeAlpha(HighlightTypeToFloat(HighlightType.Hidden));
            }
        }
    }

    private void Regenerate()
    {
        foreach (var cell in cells)
        {
            Minimap.HighlightCell(cell.x, cell.y, HighlightType.Hidden);
            for (int i = 0; i < 4; i++)
            {
                cell.SetBranch(i, false);
            }
        }
    }
}
