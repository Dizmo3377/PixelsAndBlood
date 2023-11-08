using System.IO;
using UnityEngine;

public static class JsonHelper
{
    private static bool enableDebug = false;
    public static string savePath {  get => $"{Application.persistentDataPath}/"; }
    public static void Save(ISaveableJson file)
    {
        File.WriteAllText(savePath + file.saveName, file.SaveJson());
        if (enableDebug) Debug.Log($"Saved file : {file.saveName}");
    }

    public static void Load(ISaveableJson file)
    {
        file.LoadJson(File.ReadAllText(savePath + file.saveName));
        if (enableDebug) Debug.Log($"Loaded file : {file.saveName}"); 
    }

    public static void LoadOnNextLevel(ISaveableJson file)
    {
        if (!LevelData.instance.isInitial) Load(file);
        else if (enableDebug) Debug.Log($"{file.saveName} SET TO INIT");
    }

    public static void DeleteAllData()
    {
        DirectoryInfo dir = new DirectoryInfo(savePath);
        foreach (FileInfo file in dir.GetFiles()) file.Delete();
    }
}