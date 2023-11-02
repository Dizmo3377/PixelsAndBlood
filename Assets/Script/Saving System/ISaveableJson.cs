using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveableJson
{
    public string SaveJson();
    public void LoadJson(string data);
    public string saveName { get; }
}