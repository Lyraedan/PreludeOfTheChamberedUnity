using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    public int inventorySlots = 10;
    public int currentlySelectedIndex = 0;
    public Item currentItem;
    public List<InventoryCell> cells = new List<InventoryCell>();
    public Transform inventoryCellHolder;

    /// <summary>
    /// This is our display, responsible for updating animation and playing sound (if any)
    /// </summary>
    public HeldItem held;
    [Header("UI")]
    public Text itemName;
    public PickupMenu pickupMenu;

    public static int ITEM_NONE = 0;
    public static int ITEM_POWERGLOVE = 1;
    public static int ITEM_SKATES = 2;
    public static int ITEM_CUTTERS = 3;
    public static int ITEM_POTION = 4;
    public static int ITEM_PISTOL = 5;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        Item empty = new ItemNone();
        // Assign all the slots to be empty
        for (int i = 0; i < inventorySlots; i++)
        {
            GameObject cell = Instantiate(Resources.Load("prefabs/InventoryCell") as GameObject, inventoryCellHolder);
            InventoryCell invCel = cell.GetComponent<InventoryCell>();
            invCel.item = empty;
            invCel.selected = false;
            invCel.UpdateCell();
            cells.Add(invCel);
        }
        SwitchItem(0);
    }

    void Update()
    {
        if (Player.pauseGameplay) return;

        if (Input.GetKeyDown(Player.instance.attackKey))
        {
            currentItem.Use();
            if (currentItem.id != ITEM_NONE)
            {
                held.sprite.sprite = currentItem.useSprite;
            }
        } else if(Input.GetKeyUp(Player.instance.attackKey))
        {
            if(currentItem.id != ITEM_NONE)
            {
                held.sprite.sprite = currentItem.idleSprite;
            }
        }

        // Handle item switching
        for (int i = 0; i < inventorySlots; i++)
        {
            SwitchItem(IndexToKey(i), i);
        }

        if (Input.GetKeyDown(KeyCode.P))
            AddItemToInventory(new ItemPowerGlove());
    }

    public int FindFirstFreeSlot()
    {
        for (int i = 0; i < inventorySlots; i++)
        {
            // If our inventory slot is empty
            if (cells[i].item.id == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public bool ItemExistsInInventory(Item item)
    {
        for (int i = 0; i < inventorySlots; i++)
        {
            if (cells[i].item.id == item.id)
            {
                return true;
            }
        }
        return false;
    }

    public void AddItemToInventory(Item item)
    {
        bool exists = ItemExistsInInventory(item);
        if (exists)
        {
            Debug.Log("Item exists!");
        }
        else
        {
            Debug.Log("Item does not exist. Adding!");
            pickupMenu.header.text = "You picked up " + item.name;
            pickupMenu.description.text = item.description;
            pickupMenu.Open();
            int slot = FindFirstFreeSlot();
            cells[slot].item = item;
            cells[slot].icon.sprite = cells[slot].item.icon;
            cells[slot].UpdateCell();

            if(slot == currentlySelectedIndex)
            {
                SwitchItem(slot);
            }

            Debug.Log("Added " + item.name + " to slot " + slot);
        }
    }

    public void RemoveItem(int index)
    {
        if (cells[index].item.id != 0)
        {
            cells[index].item = null;
            cells[index].item = new ItemNone();

            cells[index].icon.sprite = null;
            cells[index].UpdateCell();
        }
    }

    /// <summary>
    /// Switch item based on a key code
    /// </summary>
    /// <param name="key"></param>
    /// <param name="index"></param>
    void SwitchItem(KeyCode key, int index)
    {
        if (Input.GetKeyDown(key))
        {
            if (index != currentlySelectedIndex)
            {
                SwitchItem(index);
            }
        }
    }

    /// <summary>
    /// Core functionality of switching the item and updating sprites
    /// </summary>
    /// <param name="index"></param>
    void SwitchItem(int index)
    {
        cells[currentlySelectedIndex].selected = false;
        cells[currentlySelectedIndex].UpdateCell();

        currentlySelectedIndex = index;
        currentItem = cells[index].item;
        cells[index].selected = true;
        cells[index].UpdateCell();

        itemName.text = currentItem.name;

        UpdateHeld();
        Debug.Log("Switched to " + currentItem.name);
    }

    void UpdateHeld()
    {
        Color color = currentItem.HexToColor();
        color.a = currentItem.id != ITEM_NONE ? 1f : 0f;
        held.overlayColor = color;
        if (currentItem.id != ITEM_NONE)
        {
            held.idleSprite = currentItem.idleSprite;
            held.useSprite = currentItem.useSprite;
            held.sprite.sprite = currentItem.idleSprite;
            held.sprite.color = color;
        } else
        {
            held.idleSprite = null;
            held.useSprite = null;
            held.sprite.color = new Color(255, 255, 255, 0);
            held.sprite.sprite = null;
        }
    }

    public bool isHolding(int itemId)
    {
        return currentItem.id == itemId;
    }

    public KeyCode IndexToKey(int index)
    {
        switch (index)
        {
            case 0:
                return KeyCode.Alpha1;
            case 1:
                return KeyCode.Alpha2;
            case 2:
                return KeyCode.Alpha3;
            case 3:
                return KeyCode.Alpha4;
            case 4:
                return KeyCode.Alpha5;
            case 5:
                return KeyCode.Alpha6;
            case 6:
                return KeyCode.Alpha7;
            case 7:
                return KeyCode.Alpha8;
            case 8:
                return KeyCode.Alpha9;
            case 9:
                return KeyCode.Alpha0;
            default:
                return KeyCode.Keypad1;
        }
    }

}
