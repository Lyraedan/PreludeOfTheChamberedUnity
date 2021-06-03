using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPotion : Item
{

    public ItemPotion()
    {
        id = Inventory.ITEM_POTION;
        name = "Potion";
        description = "Healthy!";
        stackable = false;
        stackAmount = 0;
        hexCode = "4AFF47";
        idleSprite = Resources.Load<Sprite>("tex/item_potion_idle");
        useSprite = Resources.Load<Sprite>("tex/item_potion_use");
        icon = Resources.Load<Sprite>("tex/icon_potion");
    }

    public override void Use()
    {
        Debug.Log("Heal player here");
    }
}