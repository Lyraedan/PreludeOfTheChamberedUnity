using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlippers : Item
{

    public ItemFlippers()
    {
        id = Inventory.ITEM_FLIPPERS;
        name = "Flippers";
        description = "Slish splash!";
        stackable = false;
        stackAmount = 0;
        hexCode = "7CBBFF";
        idleSprite = Resources.Load<Sprite>("tex/blank");
        useSprite = Resources.Load<Sprite>("tex/blank");
        icon = Resources.Load<Sprite>("tex/icon_flippers");
    }

    public override void Use()
    {
        Debug.Log("Flippers go slish splash");
    }
}
