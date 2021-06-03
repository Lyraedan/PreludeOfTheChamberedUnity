using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkates : Item
{

    public ItemSkates()
    {
        id = Inventory.ITEM_SKATES;
        name = "Skates";
        description = "Sharp!";
        stackable = false;
        stackAmount = 0;
        hexCode = "AE70FF";
        idleSprite = Resources.Load<Sprite>("tex/blank");
        useSprite = Resources.Load<Sprite>("tex/blank");
        icon = Resources.Load<Sprite>("tex/icon_skates");
    }

    public override void Use() { }

    public override void Idle() { }
}
