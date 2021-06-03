using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCutters : Item
{

    public ItemCutters()
    {
        id = Inventory.ITEM_CUTTERS;
        name = "Cutters";
        description = "Snip, snip!!";
        stackable = false;
        stackAmount = 0;
        hexCode = "CCCCCC";
        idleSprite = Resources.Load<Sprite>("tex/item_cutters_idle");
        useSprite = Resources.Load<Sprite>("tex/item_cutters_use");
        icon = Resources.Load<Sprite>("tex/icon_cutters");
    }

    public override void Idle()
    {
        Inventory.instance.UpdateHeldToIdleSprite();
    }

    public override void Use()
    {
        Inventory.instance.UpdateHeldToUseSprite();
    }
}
