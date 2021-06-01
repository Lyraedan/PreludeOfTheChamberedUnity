using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{

    public int id = -1;
    public string name = "Item";
    public string description = string.Empty;
    public bool stackable = false;
    public int stackAmount = 0;
    public Sprite idleSprite;
    public Sprite useSprite;
    public Sprite icon;
    public string hexCode = "FFFFFFFF";

    public abstract void Use();

    public Color HexToColor()
    {
        Color color;
        if (ColorUtility.TryParseHtmlString("#" + hexCode, out color))
            return color;
        else
        {
            return Color.white;
        }
    }
}
