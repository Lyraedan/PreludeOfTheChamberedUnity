using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapLoader))]
public class MapLoaderEditor : Editor
{

    string map = string.Empty;
    int selectedLevel = 0;
    Vector3 offset = Vector3.zero;
    List<LevelEditor> maps = new List<LevelEditor>();
    List<string> mapNames = new List<string>();
    Texture2D mapDisplay;
    bool isOutside = false;
    BlockData data;
    string levelColor = string.Empty;

    private void OnEnable()
    {
        if (mapDisplay == null)
        {
            mapDisplay = Resources.Load<Texture2D>("nomap");
            mapDisplay.filterMode = FilterMode.Point;
        }
        GetLevels();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        offset = EditorGUILayout.Vector3Field("Level offset", offset);
        isOutside = EditorGUILayout.Toggle("Is outside", isOutside);
        EditorGUI.BeginChangeCheck();
        selectedLevel = EditorGUILayout.Popup("Level", selectedLevel, mapNames.ToArray());
        data = EditorGUILayout.ObjectField("Block data", data, typeof(BlockData), true) as BlockData;
        levelColor = EditorGUILayout.TextField("Level Color", levelColor);

        if (EditorGUI.EndChangeCheck())
        {
            string file = "level/" + maps[selectedLevel].fileName;
            mapDisplay = Resources.Load<Texture2D>(file);
            mapDisplay.filterMode = FilterMode.Point;
            mapDisplay.alphaIsTransparency = true;
        }

        int previewScale = 2;
        EditorGUI.DrawPreviewTexture(new Rect(10, 130, 100 * previewScale, 100 * previewScale), mapDisplay);
        GUILayout.Space((100 * previewScale) + 20);
        bool load = GUILayout.Button("Load Level into scene");
        if (load)
        {
            LoadLevelIntoScene(offset, isOutside);
        }
    }

    void LoadLevelIntoScene(Vector3 offset, bool isOutdoors = false)
    {
        int width = mapDisplay.width;
        int height = mapDisplay.height;
        Debug.Log("Importing scene of size: " + width + ", " + height + " total tiles = " + (width * height));
        GameObject map = new GameObject("Map_" + maps[selectedLevel].name);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = mapDisplay.GetPixel(x, y);
                if (pixel.a != 0)
                {
                    string hex = ColorUtility.ToHtmlStringRGB(pixel);
                    if (hexIsPlane(hex))
                    {
                        //Floor
                        GameObject plane = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), map.transform);
                        Block block = plane.GetComponent<Block>();
                        block.color = pixel;
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        if (texture != null)
                        {
                            block.texture = texture;
                        }
                        block.loadProperties();
                    }
                    else if (hexIsEntity(hex))
                    {
                        // Spawn the floor the entity will stand on
                        GameObject plane = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), map.transform);
                        Block block = plane.GetComponent<Block>();
                        block.color = pixel;
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        block.loadProperties();

                        // Spawn entity
                        string entity = hexToEntity(hex);
                        plane = PlaceEntityAt(new Vector3(offset.x + (1 * x), offset.y + 0.5f, offset.z + (1 * y)), map.transform, entity);
                        block = plane.GetComponent<Block>();
                        block.color = pixel;
                        block.type = Block.BlockType.ENTITY;
                        block.hex = hex;
                        block.loadProperties();
                    }
                    else
                    {
                        GameObject cube = PlaceBlockAt(new Vector3(offset.x + (1 * x), offset.y + 0.5f, offset.z + (1 * y)), map.transform);
                        Block block = cube.GetComponent<Block>();
                        block.color = pixel;
                        block.type = Block.BlockType.WALL;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        if (texture != null)
                        {
                            block.texture = texture;
                        }
                        block.loadProperties();
                    }

                    // Spawn the ceiling
                    if (!isOutdoors)
                    {
                        GameObject roof = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), map.transform, 180);
                        Block roofBlock = roof.GetComponent<Block>();
                        roofBlock.color = pixel;
                        roofBlock.type = Block.BlockType.FLOOR;
                        roofBlock.hex = hex;
                        roofBlock.loadProperties();
                    }
                }
            }
        }
    }

    GameObject PlaceBlockAt(Vector3 pos, Transform parent)
    {
        GameObject cube = Instantiate(Resources.Load("prefabs/Cube") as GameObject, parent);
        cube.transform.position = pos;
        return cube;
    }

    GameObject PlaceEntityAt(Vector3 pos, Transform parent, string entity)
    {
        GameObject cube = Instantiate(Resources.Load("prefabs/" + entity) as GameObject, parent);
        cube.transform.position = pos;
        return cube;
    }

    GameObject PlacePlaneAt(Vector3 pos, Transform parent, float rotation = 0)
    {
        GameObject plane = Instantiate(Resources.Load("prefabs/Plane") as GameObject, parent);
        plane.transform.position = pos;
        plane.transform.Rotate(new Vector3(0, 0, 1), rotation);
        return plane;
    }

    void GetLevels()
    {
        maps.Clear();
        mapNames.Clear();
        string myPath = "Assets/Resources/level";
        DirectoryInfo dir = new DirectoryInfo(myPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            if (f.Extension == ".png")
            {
                string n = f.Name.Substring(1, f.Name.Length - 5);
                string mapName = char.ToUpper(f.Name[0]) + n;
                LevelEditor lvl = new LevelEditor();
                lvl.fileName = f.Name.Substring(0, f.Name.Length - 4);
                lvl.name = mapName;
                lvl.dir = f.FullName;
                maps.Add(lvl);
                mapNames.Add(mapName);
            }
        }
    }

    bool hexIsPlane(string hex)
    {
        return hex.Equals("000000") || hex.Equals("3F3F60") || hex.Equals("0000FF") || hex.Equals("653A00") || hex.Equals("009300");
    }

    bool hexIsEntity(string hex)
    {
        return /*hex.Equals("FF0000") || */hex.Equals("FF3A02")/* || hex.Equals("AA5500")*/;
    }

    string hexToEntity(string hex)
    {
        switch (hex)
        {
            case "FF3A02": // Torch
                return "Light";
            default:
                throw new ArgumentException("Found hex marked as entity but found no assosiated entity");
        }
    }

    Texture GetTexture(string hex)
    {
        foreach (BlockEntry entry in data.blocks)
        {
            if (entry.hex.Equals(hex))
                return entry.texture;
        }
        return null;
    }
}

class LevelEditor
{
    public string fileName = string.Empty;
    public string name = string.Empty;
    public string dir = string.Empty;
}