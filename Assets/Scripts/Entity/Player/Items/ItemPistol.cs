using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPistol : Item
{

    public ItemPistol()
    {
        id = Inventory.ITEM_PISTOL;
        name = "Pistol";
        description = "Pew, pew, pew!";
        stackable = true;
        stackAmount = 0;
        ammo = 30;
        hexCode = "EAEAEA";
        idleSprite = Resources.Load<Sprite>("tex/item_pistol_idle");
        useSprite = Resources.Load<Sprite>("tex/item_pistol_use");
        icon = Resources.Load<Sprite>("tex/icon_pistol");
        useSfx = Resources.Load<AudioClip>("snd/shoot");
    }

    public override void Use()
    {
        if (stackAmount > 0)
        {
            EventManager.instance.ExecuteCoroutine(Fire());
        }
    }

    public override void Idle() { }

    IEnumerator Fire()
    {
        Inventory.instance.UpdateHeldToUseSprite();
        Player.instance.itemAudioSource.clip = useSfx;
        Player.instance.itemAudioSource.Play();

        GameObject projectileObject = EventManager.instance.InstantiateGameObject("prefabs/Projectile");
        projectileObject.transform.position = Player.instance.transform.position + (Player.instance.transform.forward);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.speed = 300f;
        projectile.Launch(Player.instance.gameObject.transform.forward);

        RemoveFromStack(1);
        UpdateStackCountDisplay();
        yield return new WaitUntil(() => !Player.instance.itemAudioSource.isPlaying);
        Inventory.instance.UpdateHeldToIdleSprite();
    }
}