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
    public enum NavMeshArea
    {
        Walkable = 0,
        NotWalkable = 1,
        Jump = 2,
        Trigger = 3,
        Water = 4
    }
    public string id = string.Empty;
    public string hex = string.Empty;
    public Texture texture;
    public Texture normalMap;
    public Texture bumpMap;
    public Texture specularMap;
    [Tooltip("What texture should the ceiling use if any? Default = Floor")] public Texture roofTexture;
    public bool useWallColor = false;
    public bool useFloorColor = false;
    public bool useCeilingColor = false;
    public Color overlayColor = Color.white;
    public bool hasTrigger = false;
    public NavMeshArea navmeshArea = NavMeshArea.NotWalkable;
}