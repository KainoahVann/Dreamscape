using System;
using UnityEngine;

[Serializable]
public class FoodDefinition
{
    public ItemType itemType;
    public string   displayName;
    [Tooltip("How much hunger this restores (out of 100)")]
    public float    hungerRestore = 20f;
}