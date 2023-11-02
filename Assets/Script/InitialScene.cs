using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//IT IS NOT SIGLETON! IT SHOULDNT BE ONE!
public class InitialScene : MonoBehaviour
{
    private void Start() 
    {
        JsonHelper.DeleteAllData();
        Inventory.slots[0] = null;
        Inventory.slots[1] = null;
        LevelData.instance.lvl = 1;
        LevelData.instance.stage = 1;
    }
}