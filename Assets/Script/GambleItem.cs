using System;
using UnityEngine;

[Serializable]
public class GambleItem : MonoBehaviour
{
    [Range(1, 100)] public int dropChance;
    public GameObject dropObject;
}