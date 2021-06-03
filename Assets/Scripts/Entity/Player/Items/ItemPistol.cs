using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPistol : Item
{

    public ItemPistol()
    {
        id = Inventory.ITEM_PISTOL;
        name = "Pistol";
        description = "Pew, pew, pew!";
        stackable = false;
        stackAmount = 0;
        ammo = 30;
        hexCode = "EAEAEA";
        idleSprite = Resources.Load<Sprite>("tex/item_pistol_idle");
        useSprite = Resources.Load<Sprite>("tex/item_pistol_use");
        icon = Resources.Load<Sprite>("tex/icon_pistol");
    }

    public override void Use()
    {
        Debug.Log("Shoot pistol here");
    }
}