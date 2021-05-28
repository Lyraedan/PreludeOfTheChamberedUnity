using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public static string levelName = string.Empty;
    public static string fileName = string.Empty;
    public static Texture floor;
    public static Texture ceil;
    public static BlockData data;
    public static Color wallColor;
    public static Color floorColor;
    public static Color ceilColor;
    public static Texture2D mapDisplay;
    public static int selectedLevel = 0;
    public static string map = string.Empty;
    public static Vector3 offset = Vector3.zero;
    public static bool isOutside = false;
}
