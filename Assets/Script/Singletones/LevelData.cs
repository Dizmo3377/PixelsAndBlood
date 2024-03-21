using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelData : Singletone<LevelData>, ISaveableJson
{
    public int lvl;
    public int stage;
    public string saveName => "LevelData";
    public bool isInitial => FindObjectOfType<InitialScene>() != null;

    private void OnEnable() => SceneController.onTransitionStart += IterateLevelData;
    private void OnDisable() => SceneController.onTransitionStart -= IterateLevelData;

    private void Start() => JsonHelper.LoadOnNextLevel(this);

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