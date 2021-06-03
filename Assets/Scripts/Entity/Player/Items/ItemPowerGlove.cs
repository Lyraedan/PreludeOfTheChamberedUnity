using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPowerGlove : Item
{

    public ItemPowerGlove()
    {
        id = Inventory.ITEM_POWERGLOVE;
        name = "Powerglove";
        description = "Smaaaash!!";
        stackable = false;
        stackAmount = 0;
        hexCode = "FFC363";
        idleSprite = Resources.Load<Sprite>("tex/item_powerglove_idle");
        useSprite = Resources.Load<Sprite>("tex/item_powerglove_use");
        icon = Resources.Load<Sprite>("tex/icon_powerglove");
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
