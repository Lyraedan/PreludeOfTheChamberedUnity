using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKey : Item
{

    public ItemKey()
    {
        id = Inventory.ITEM_KEY;
        name = "Key";
        description = "How did you get this?";
        stackable = false;
        stackAmount = 0;
        hexCode = "FF4040";
        idleSprite = Resources.Load<Sprite>("tex/item_key_idle");
        useSprite = Resources.Load<Sprite>("tex/item_key_use");
        icon = Resources.Load<Sprite>("tex/icon_key");
    }

    public override void Use()
    {
        Inventory.instance.UpdateHeldToUseSprite();
    }

    public override void Idle()
    {
        Inventory.instance.UpdateHeldToIdleSprite();
    }
}