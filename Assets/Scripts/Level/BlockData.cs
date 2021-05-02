using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "ScriptableObjects/BlockData", order = 1)]
public class BlockData : ScriptableObject
{
    public List<BlockEntry> blocks = new List<BlockEntry>();
}

[Serializable]
public class BlockEntry
{
    public string id = string.Empty;
    public string hex = string.Empty;
    public Texture texture;
    public Color overlayColor = Color.white;
    public bool hasTrigger = false;
}