using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Chest : MonoBehaviour
{

    public enum ChestLoot
    {
        EMPTY,
        POWERGLOVE,
        SKATES,
        CUTTERS,
        POTION,
        PISTOL,
        FLIPPERS
    }

    public ChestLoot treasure = ChestLoot.EMPTY;
    [Space(5)]
    public new Renderer renderer;
    public AudioClip openSfx;
    public UnityEvent OnOpened;
    public Texture openedTexture;

    private AudioSource src;

    public bool opened = false;

    void Start()
    {
        src = GetComponent<AudioSource>();    
    }

    public void Open()
    {
        if (opened) return;

        OnOpened?.Invoke();
        renderer.material.mainTexture = openedTexture;
        src.clip = openSfx;
        src.Play();
        Inventory.instance.AddItemToInventory(LootItem());
        opened = true;
    }

    Item LootItem()
    {
        switch(treasure)
        {
            case ChestLoot.POWERGLOVE:
                return new ItemPowerGlove();
            default:
                return new ItemNone();
        }
    }

}
