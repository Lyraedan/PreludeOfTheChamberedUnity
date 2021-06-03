using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public Item item;
    public Image icon;
    public GameObject selectedBox;
    public Text stack;
    public bool selected = false;

    public void UpdateCell()
    {
        selectedBox.SetActive(selected);
        stack.gameObject.SetActive(item.stackable);
        stack.text = $"{item.stackAmount}";
        Color color = item.HexToColor();
        color.a = item.id != Inventory.ITEM_NONE ? 1f : 0f;
        icon.color = color;
    }

}
