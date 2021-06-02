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
    }

    public override void Use()
    {
        Debug.Log("Nothing was used!");
    }
}
