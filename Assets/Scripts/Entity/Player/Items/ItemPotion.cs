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
        stackable = true;
        stackAmount = 0;
        hexCode = "4AFF47";
        idleSprite = Resources.Load<Sprite>("tex/item_potion_idle");
        useSprite = Resources.Load<Sprite>("tex/item_potion_use");
        icon = Resources.Load<Sprite>("tex/icon_potion");
        useSfx = Resources.Load<AudioClip>("snd/potion");
    }

    public override void Use()
    {
        if (stackAmount > 0)
        {
            Inventory.instance.UpdateHeldToUseSprite();
            Player.instance.itemAudioSource.clip = useSfx;
            Player.instance.itemAudioSource.Play();
            Player.instance.Heal(3);
            RemoveFromStack(1);
            UpdateStackCountDisplay();
        }
    }

    public override void Idle()
    {
        Inventory.instance.UpdateHeldToIdleSprite();
    }
}