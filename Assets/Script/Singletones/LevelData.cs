using UnityEngine;

public class LevelData : Singletone<LevelData>, ISaveableJson
{
    public int lvl;
    public int stage;

    public string saveName => "LevelData";
    public bool isInitial => FindFirstObjectByType<InitialScene>() != null;

    public override void Awake()
    {
        base.Awake();
        JsonHelper.LoadOnNextLevel(this);

        if (isInitial)
        {
            instance.lvl = 1;
            instance.stage = 1;
        }
    }

    public void IterateLevelData()
    {
        lvl++;
        if (lvl >= 6)
        {
            stage++;
            lvl = 1;
        }
        JsonHelper.Save(this);
    }

    public string SaveJson() => JsonUtility.ToJson(this, true);

    public void LoadJson(string data) => JsonUtility.FromJsonOverwrite(data, this);
}