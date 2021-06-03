using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNone : Item
{
    public ItemNone()
    {
        id = Inventory.ITEM_NONE;
        name = "";
        stackable = false;
        stackAmount = 0;
        hexCode = "000000";
        idleSprite = Resources.Load<Sprite>("tex/blank");
        useSprite = Resources.Load<Sprite>("tex/blank");
        icon = Resources.Load<Sprite>("tex/blank");
    }

    public override void Use() { }

    public override void Idle() { }
}
