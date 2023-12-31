using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatch : InteractObject
{
    protected override void OnInteract() => Transition();
    private void Transition()
    {
        int lvl = LevelData.instance.lvl + 1;
        int stage = lvl >= 6 ? LevelData.instance.stage + 1 : LevelData.instance.stage;

        SceneController.instance.StartSceneTransition($"Dungeon_{stage}");
    }
}