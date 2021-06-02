/*
 
    Author: Zel
 
 */
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
    List<LevelEditor> maps = new List<LevelEditor>();
    List<string> mapNames = new List<string>();

    private void OnEnable()
    {
        if (MapLoader.mapDisplay == null)
        {
            MapLoader.mapDisplay = Resources.Load<Texture2D>("nomap");
            MapLoader.mapDisplay.filterMode = FilterMode.Point;
        }
        GetLevels();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapLoader.levelName = EditorGUILayout.TextField("Level Name:", MapLoader.levelName);
        MapLoader.offset = EditorGUILayout.Vector3Field("Level offset", MapLoader.offset);
        MapLoader.isOutside = EditorGUILayout.Toggle("Is outside", MapLoader.isOutside);
        EditorGUI.BeginChangeCheck();
        MapLoader.selectedLevel = EditorGUILayout.Popup("Level", MapLoader.selectedLevel, mapNames.ToArray());
        MapLoader.data = EditorGUILayout.ObjectField("Block data", MapLoader.data, typeof(BlockData), true) as BlockData;
        MapLoader.wallColor = EditorGUILayout.ColorField("Wall Color", MapLoader.wallColor);
        MapLoader.floorColor = EditorGUILayout.ColorField("Floor Color", MapLoader.floorColor);
        MapLoader.ceilColor = EditorGUILayout.ColorField("Ceil Color", MapLoader.ceilColor);
        //Todo fix
        EditorGUILayout.BeginHorizontal();
        MapLoader.floor = TextureField("Floor", MapLoader.floor);
        MapLoader.ceil = TextureField("Ceiling", MapLoader.ceil);
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            string file = "level/" + maps[MapLoader.selectedLevel].fileName;
            MapLoader.mapDisplay = Resources.Load<Texture2D>(file);
            MapLoader.mapDisplay.filterMode = FilterMode.Point;
            MapLoader.mapDisplay.alphaIsTransparency = true;
            LoadProfile();
        }

        int previewScale = 2;
        EditorGUI.DrawPreviewTexture(new Rect(10, 285, 100 * previewScale, 100 * previewScale), MapLoader.mapDisplay);
        GUILayout.Space((100 * previewScale) + 20);
        bool profile = GUILayout.Button("Load Level profile");
        if (profile)
        {
            LoadProfile();
        }
        bool load = GUILayout.Button("Load Level into scene");
        if (load)
        {
            LoadLevelIntoScene(MapLoader.offset, MapLoader.isOutside);
        }
        bool link = GUILayout.Button("Link levels");
        if(link)
        {
            SetupLinkedObjects();
        }
    }

    JSONNode LoadProfileNodes(string level)
    {
        TextAsset file = Resources.Load<TextAsset>($"profiles/{level}");
        if (file != null)
        {
            JSONNode node = JSON.Parse(file.text);
            return node;
        }
        return null;
    }

    void LoadProfile()
    {
        string level = maps[MapLoader.selectedLevel].fileName;
        JSONNode node = LoadProfileNodes(level);
        if (node != null)
        {
            string levelName = node["name"];
            MapLoader.levelName = levelName;
            MapLoader.fileName = level;
            string wallCol = "#" + node["wallCol"];
            string floorCol = "#" + node["floorCol"];
            string ceilCol = "#" + node["ceilCol"];
            var floorTex = node["floorTex"];
            var ceilTex = node["ceilTex"];
            bool isOutside = node["outside"];
            var data = node["data"];
            Vector3 offset = new Vector3(node["offset"]["x"], node["offset"]["y"], node["offset"]["z"]);
            MapLoader.offset = offset;
            Color wall, floor, ceil;
            if (ColorUtility.TryParseHtmlString(wallCol, out wall))
            {
                MapLoader.wallColor = wall;
            }
            if (ColorUtility.TryParseHtmlString(floorCol, out floor))
            {
                MapLoader.floorColor = floor;
            }
            if (ColorUtility.TryParseHtmlString(ceilCol, out ceil))
            {
                MapLoader.ceilColor = ceil;
            }
            if(!string.IsNullOrEmpty(floorTex))
            {
                MapLoader.floor = Resources.Load<Texture>(floorTex);
            }
            if(!string.IsNullOrEmpty(ceilTex))
            {
                MapLoader.ceil = Resources.Load<Texture>(ceilTex);
            }
            MapLoader.isOutside = isOutside;
            if(!string.IsNullOrEmpty(data))
            {
                MapLoader.data = Resources.Load<BlockData>(data);
            }
        }
        else
        {
            MapLoader.wallColor = Color.white;
            MapLoader.floorColor = Color.white;
            MapLoader.ceilColor = Color.white;
        }
    }

    void LoadLevelIntoScene(Vector3 offset, bool isOutdoors = false)
    {
        int width = MapLoader.mapDisplay.width;
        int height = MapLoader.mapDisplay.height;
        Debug.Log("Importing scene of size: " + width + ", " + height + " total tiles = " + (width * height));
        GameObject map = new GameObject("Map_" + maps[MapLoader.selectedLevel].name);
        LevelDetails details = map.AddComponent<LevelDetails>();
        details.levelName = MapLoader.levelName;
        details.levelId = MapLoader.selectedLevel;
        details.fileName = MapLoader.fileName;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = MapLoader.mapDisplay.GetPixel(x, y);
                if (pixel.a != 0)
                {
                    string hex = ColorUtility.ToHtmlStringRGB(pixel);
                    if (hexIsPlane(hex))
                    {
                        //Floor
                        GameObject plane = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), map.transform);
                        plane.name = "Floor";
                        Block block = plane.GetComponent<Block>();
                        bool hasEntry = HasEntry(hex);
                        block.color = hasEntry ? GetOverlayColor(hex) : MapLoader.floorColor; // pixel
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        block.texture = texture == null ? MapLoader.floor : texture;

                        bool hasTrigger = HasTrigger(hex);
                        if (hasTrigger)
                        {
                            GameObject trigger = PlaceTriggerAt(new Vector3(offset.x + (1 * x), offset.y, offset.z + (1 * y)), plane.transform);
                            trigger.name = "Trigger";
                        }

                        block.loadProperties();

                        if (!isOutdoors)
                        {
                            // Roof
                            GameObject roof = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y + 1f, offset.z + (1 * y)), map.transform, 180);
                            roof.name = "Roof";
                            Block roofBlock = roof.GetComponent<Block>();
                            roofBlock.color = MapLoader.ceilColor;
                            roofBlock.type = Block.BlockType.FLOOR;
                            roofBlock.hex = hex;
                            Texture roofTexture = GetTexture(hex, true);
                            roofBlock.texture = roofTexture == null ? MapLoader.floor : roofTexture;
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
                        if (entityBlock == null)
                        {
                            if (e.transform.childCount > 0)
                            {
                                // Incase we put a collider as the root object check the first child
                                entityBlock = e.transform.GetChild(0).gameObject.GetComponent<Block>();
                            }
                            else
                            {
                                Debug.Log("No Block script found on entity and no children found");
                            }
                        }
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
                        block.color = MapLoader.floorColor; // pixel
                        block.type = Block.BlockType.FLOOR;
                        block.hex = hex;
                        Texture texture = GetTexture(hex);
                        if (e.GetComponent<LadderBridge>())
                        {
                            LadderBridge ladder = e.GetComponent<LadderBridge>();
                            if (ladder.direction.Equals(LadderBridge.Direction.DOWN))
                                block.texture = ladder.texture;
                            else
                                block.texture = MapLoader.floor;
                        }
                        else
                            block.texture = MapLoader.floor;

                        block.loadProperties();

                        // Spawn the ceiling if any
                        if (!isOutdoors)
                        {
                            GameObject roof = PlacePlaneAt(new Vector3(offset.x + (1 * x), offset.y + 1f, offset.z + (1 * y)), map.transform, 180);
                            roof.name = "Roof";
                            Block roofBlock = roof.GetComponent<Block>();
                            if (roofBlock == null)
                            {
                                if (roof.transform.childCount > 0)
                                {
                                    // Incase we put a collider as the root object check the first child
                                    roofBlock = roof.transform.GetChild(0).gameObject.GetComponent<Block>();
                                }
                                else
                                {
                                    Debug.Log("No Block script found on entity and no children found");
                                }
                            }
                            roofBlock.color = MapLoader.ceilColor;
                            roofBlock.type = Block.BlockType.CEILING;
                            roofBlock.hex = hex;
                            //Texture roofTexture = GetTexture(hex);
                            if (roof.GetComponent<LadderBridge>())
                            {
                                LadderBridge ladder = roof.GetComponent<LadderBridge>();
                                if (ladder.direction.Equals(LadderBridge.Direction.UP))
                                    roofBlock.texture = ladder.texture;
                                else
                                    roofBlock.texture = MapLoader.ceil;
                            }
                            else
                                roofBlock.texture = MapLoader.ceil;
                            roofBlock.loadProperties();
                        }
                    }
                    else
                    {
                        GameObject cube = PlaceBlockAt(new Vector3(offset.x + (1 * x), offset.y + 0.5f, offset.z + (1 * y)), map.transform);
                        Block block = cube.GetComponent<Block>();
                        bool hasEntry = HasEntry(hex);
                        block.color = hasEntry ? GetOverlayColor(hex) : MapLoader.wallColor; // pixel
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
        //SetupLinkedObjects();
    }

    void SetupLinkedObjects()
    {
        var ladders = FindObjectsOfType<LadderBridge>();
        var levels = FindObjectsOfType<LevelDetails>();

        for (int i = 0; i < ladders.Length; i++)
        {
            LadderBridge ladder = ladders[i];
            ladder.id = i;
            ladder.gameObject.name = $"Ladder_{i}";
            Debug.Log("Trying to link ladder " + i);
            //foreach(LevelDetails level in levels)
            //{
                JSONNode node = LoadProfileNodes(MapLoader.fileName);
                Debug.Log("Loaded nodes: " + node.ToString());
                var links = node["links"];
                for(int j = 0; j < links.Count; j++)
                {
                    var nextLadderId = links[$"ladder_{j}"]["goto"];
                    GameObject linkedLadder = GameObject.Find($"Ladder_{nextLadderId}");
                    if (!linkedLadder)
                    {
                        Debug.LogError("Failed to find ladder " + nextLadderId);
                    } else
                    {
                        ladder.teleportTo = linkedLadder.transform; 
                    }
                }
            //}
        }
    }

    GameObject PlaceBlockAt(Vector3 pos, Transform parent)
    {
        GameObject cube = Instantiate(Resources.Load("prefabs/Cube") as GameObject, parent);
        cube.transform.position = pos;
        return cube;
    }

    GameObject PlaceTriggerAt(Vector3 pos, Transform parent)
    {
        GameObject trigger = Instantiate(Resources.Load("prefabs/Trigger") as GameObject, parent);
        trigger.transform.position = new Vector3(pos.x, pos.y + 0.5f, pos.z);
        trigger.transform.localScale = new Vector3(10f, 1f, 10f);
        return trigger;
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
               hex.Equals("653A00") || hex.Equals("009300") || hex.Equals("009380") ||
               hex.Equals("0000FF") || hex.Equals("653A00") || hex.Equals("3F3F60");
    }

    bool hexIsEntity(string hex)
    {
        return hex.Equals("4C4C4C") || hex.Equals("FF3A02") || hex.Equals("FF0000") ||
               hex.Equals("AA5500") || hex.Equals("9E009E") || hex.Equals("009380") ||
               hex.Equals("FF66FF") || hex.Equals("FFFF64") || hex.Equals("C1C14D") ||
               hex.Equals("FFFF00") || hex.Equals("FF0001") || hex.Equals("00FFFF") ;
    }

    string hexToEntity(string hex)
    {
        switch (hex)
        {
            case "FF3A02":
                return "Light";
            case "4C4C4C":
                return "Bars";
            case "FF0000":
                return "Bat";
            case "FF0001":
                return "BossBat";
            case "AA5500":
                return "Bolder";
            case "009300":
                return "BolderHole";
            case "FF66FF":
                return "LadderUp";
            case "9E009E":
                return "LadderDown";
            case "FFFF64":
                return "Chest";
            case "C1C14D":
                return "Loot";
            case "FFFF00":
                return "Spawn";
            case "00FFFF":
                return "BreakableBlock";
            default:
                throw new ArgumentException("Found hex marked as entity but found no assosiated entity");
        }
    }

    Texture GetTexture(string hex, bool useRoof = false)
    {
        foreach (BlockEntry entry in MapLoader.data.blocks)
        {
            if (entry.hex.Equals(hex))
            {
                if (!useRoof)
                    return entry.texture;
                else
                    return entry.roofTexture;
            }
        }
        return null;
    }

    Color GetOverlayColor(string hex)
    {
        foreach (BlockEntry entry in MapLoader.data.blocks)
        {
            if (entry.hex.Equals(hex))
            {
                if (entry.useWallColor)
                {
                    return MapLoader.wallColor;
                }
                else if(entry.useFloorColor)
                {
                    return MapLoader.floorColor;
                }
                else if(entry.useCeilingColor)
                {
                    return MapLoader.ceilColor;
                }
                else
                {
                    return entry.overlayColor;
                }
            }
        }
        return Color.white;
    }

    bool HasEntry(string hex)
    {
        foreach (BlockEntry entry in MapLoader.data.blocks)
        {
            if (entry.hex.Equals(hex))
            {
                return true;
            }
        }
        return false;
    }

    bool HasTrigger(string hex)
    {
        foreach (BlockEntry entry in MapLoader.data.blocks)
        {
            if (entry.hex.Equals(hex))
                return entry.hasTrigger;
        }
        return false;
    }

    private static Texture TextureField(string name, Texture texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        var result = (Texture)EditorGUILayout.ObjectField(texture, typeof(Texture), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }
}

class LevelEditor
{
    public string fileName = string.Empty;
    public string name = string.Empty;
    public string dir = string.Empty;
}