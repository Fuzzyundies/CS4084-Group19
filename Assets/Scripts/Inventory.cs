using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
   // public int space = 21;
    public PlayerController player;
    public GameObject inventoryMenu;
    private Item[] craftingQueue = new Item[2];

    public void Add(Item item)
    {
        items.Add(item);
        player.UpdateStats();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
    }

    public void InventoryAccess()
    {
        if (inventoryMenu.activeSelf == true)
            inventoryMenu.SetActive(false);
        else
            inventoryMenu.SetActive(true);
    }
}
