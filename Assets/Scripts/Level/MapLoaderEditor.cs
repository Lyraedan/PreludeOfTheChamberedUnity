using SimpleJSON;
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
    Color wallColor = Color.white;
    Color floorColor = Color.white;
    Color ceilColor = Color.white;

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
        wallColor = EditorGUILayout.ColorField("Wall Color", wallColor);
        floorColor = EditorGUILayout.ColorField("Floor Color", floorColor);
        ceilColor = EditorGUILayout.ColorField("Ceil Color", ceilColor);

        if (EditorGUI.EndChangeCheck())
        {
            string file = "level/" + maps[selectedLevel].fileName;
            mapDisplay = Resources.Load<Texture2D>(file);
            mapDisplay.filterMode = FilterMode.Point;
            mapDisplay.alphaIsTransparency = true;
            LoadProfile();
        }

        int previewScale = 2;
        EditorGUI.DrawPreviewTexture(new Rect(10, 170, 100 * previewScale, 100 * previewScale), mapDisplay);
        GUILayout.Space((100 * previewScale) + 20);
        bool profile = GUILayout.Button("Load Level profile");
        if (profile)
        {
            LoadProfile();
        }
        bool load = GUILayout.Button("Load Level into scene");
        if (load)
        {
            LoadLevelIntoScene(offset, isOutside);
        }
    }

    void LoadProfile()
    {
        string level = maps[selectedLevel].fileName;
        TextAsset file = Resources.Load<TextAsset>($"profiles/{level}");
        if (file != null)
        {
            Debug.Log("Loading profile for " + level);
            JSONNode node = JSON.Parse(file.text);
            string wallCol = "#" + node["wallCol"];
            string floorCol = "#" + node["floorCol"];
            string ceilCol = "#" + node["ceilCol"];
            Debug.Log(wallCol);
            Debug.Log(floorCol);
            Debug.Log(ceilCol);
            Color wall, floor, ceil;
            if (ColorUtility.TryParseHtmlString(wallCol, out wall))
            {
                wallColor = wall;
            }
            if (ColorUtility.TryParseHtmlString(floorCol, out floor))
            {
                floorColor = floor;
            }
            if (ColorUtility.TryParseHtmlString(ceilCol, out ceil))
            {
                ceilColor = ceil;
            }
            Debug.Log("Successfully loaded profile for " + level);
        }
        else
        {
            Debug.LogError("Failed to find profile for " + level);
            wallColor = Color.white;
            floorColor = Color.white;
            ceilColor = Color.white;
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
                        plane.name = "Floor";
                        Block block = plane.GetComponent<Block>();
                        block.color = !isOutside ? floorColor : Color.white; // pixel
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        if (texture != null)
                        {
                            block.texture = texture;
                        }
                        block.loadProperties();

                        if (!isOutdoors)
                        {
                            // Roof
                            GameObject roof = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y + 1f, offset.z + (1 * y)), map.transform, 180);
                            roof.name = "Roof";
                            Block roofBlock = roof.GetComponent<Block>();
                            roofBlock.color = ceilColor;
                            roofBlock.type = Block.BlockType.FLOOR;
                            roofBlock.hex = hex;
                            texture = GetTexture("000000"); // Floor hex
                            if (texture != null)
                            {
                                roofBlock.texture = texture;
                            }
                            roofBlock.loadProperties();
                        }
                    }
                    else if (hexIsEntity(hex))
                    {
                        // Spawn entity
                        string entity = hexToEntity(hex);
                        GameObject e = PlaceEntityAt(new Vector3(offset.x + (1 * x), offset.y + 0.5f, offset.z + (1 * y)), map.transform, entity);
                        e.name = "Entity";
                        Block entityBlock = e.GetComponent<Block>();
                        entityBlock.color = GetOverlayColor(hex); // pixel
                        entityBlock.type = Block.BlockType.ENTITY;
                        entityBlock.hex = hex;
                        Texture entityTexture = GetTexture(hex);
                        if (entityTexture != null)
                        {
                            entityBlock.texture = entityTexture;
                            entityBlock.renderMode = 1;
                        }
                        entityBlock.loadProperties();

                        // Spawn the floor the entity will stand on
                        GameObject plane = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), map.transform);
                        Block block = plane.GetComponent<Block>();
                        block.color = floorColor; // pixel
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        Texture texture = GetTexture("000000"); // Floor hex
                        if (texture != null)
                        {
                            if (entityBlock.GetComponent<LadderBridge>())
                            {
                                LadderBridge ladder = entityBlock.GetComponent<LadderBridge>();
                                if (ladder.direction.Equals(LadderBridge.Direction.DOWN))
                                    block.texture = ladder.texture;
                                else
                                    block.texture = texture;
                            }
                            else
                                block.texture = texture;
                        }
                        block.loadProperties();

                        // Spawn the ceiling if any
                        if (!isOutdoors)
                        {
                            GameObject roof = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y + 1f, offset.z + (1 * y)), map.transform, 180);
                            roof.name = "Roof";
                            Block roofBlock = roof.GetComponent<Block>();
                            roofBlock.color = ceilColor;
                            roofBlock.type = Block.BlockType.FLOOR;
                            roofBlock.hex = hex;
                            texture = GetTexture("000000"); // Floor hex
                            if (entityBlock.GetComponent<LadderBridge>())
                            {
                                LadderBridge ladder = entityBlock.GetComponent<LadderBridge>();
                                if (ladder.direction.Equals(LadderBridge.Direction.UP))
                                    roofBlock.texture = ladder.texture;
                                else
                                    roofBlock.texture = texture;
                            }
                            else
                                roofBlock.texture = texture;
                            roofBlock.loadProperties();
                        }
                    }
                    else
                    {
                        GameObject cube = PlaceBlockAt(new Vector3(offset.x + (1 * x), offset.y + 0.5f, offset.z + (1 * y)), map.transform);
                        Block block = cube.GetComponent<Block>();
                        block.color = wallColor; // pixel
                        block.type = Block.BlockType.WALL;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        if (texture != null)
                        {
                            block.texture = texture;
                        }
                        block.loadProperties();
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
        return hex.Equals("000000") || hex.Equals("3F3F60") || hex.Equals("0000FF") ||
               hex.Equals("653A00") || hex.Equals("009300");
    }

    bool hexIsEntity(string hex)
    {
        return hex.Equals("4C4C4C") || hex.Equals("FF3A02") || hex.Equals("FF0000") ||
               hex.Equals("AA5500") || hex.Equals("009300") || hex.Equals("9E009E") ||
               hex.Equals("FF66FF");
    }

    string hexToEntity(string hex)
    {
        switch (hex)
        {
            case "FF3A02": // Torch
                return "Light";
            case "4C4C4C":
                return "Bars";
            case "FF0000":
                return "Bat";
            case "AA5500":
                return "Bolder";
            case "009300":
                return "BolderHole";
            case "9E009E":
                return "LadderUp";
            case "FF66FF":
                return "LadderDown";
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

    Color GetOverlayColor(string hex)
    {
        foreach (BlockEntry entry in data.blocks)
        {
            if (entry.hex.Equals(hex))
                return entry.overlayColor;
        }
        return Color.white;
    }
}

class LevelEditor
{
    public string fileName = string.Empty;
    public string name = string.Empty;
    public string dir = string.Empty;
}