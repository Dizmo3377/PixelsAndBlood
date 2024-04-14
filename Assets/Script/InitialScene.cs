using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitialScene : MonoBehaviour
{
    private void Awake()
    {
        JsonHelper.DeleteAllData();
        Inventory.slots[0] = null;
        Inventory.slots[1] = null;
    }
}