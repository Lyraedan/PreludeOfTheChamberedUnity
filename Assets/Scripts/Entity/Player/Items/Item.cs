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
    public int ammo = 1;
    public Sprite idleSprite;
    public Sprite useSprite;
    public Sprite icon;
    public string hexCode = "FFFFFFFF";
    public AudioClip useSfx;

    /// <summary>
    /// What happens when we use this item?
    /// </summary>
    public abstract void Use();

    /// <summary>
    /// What happens after being used
    /// </summary>
    public abstract void Idle();

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

    public void UpdateStackCountDisplay()
    {
        int slot = Inventory.instance.GetItemSlot(id);
        Inventory.instance.cells[slot].UpdateCell();
    }

    public void RemoveFromStack(int amt)
    {
        stackAmount -= amt;

        if (stackAmount < 0)
            stackAmount = 0;
    }
}
