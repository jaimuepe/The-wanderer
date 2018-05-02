using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory
{
    static List<string> items = new List<string>();

    static Inventory()
    {
        AddItem("jar1");
        AddItem("jar2");
        AddItem("jar3");
        AddItem("jar4");

        AddItem("jar1");
        AddItem("jar2");
        AddItem("jar3");
        AddItem("jar4");
    }

    public static void AddItem(string id)
    {
        items.Add(id);
        Debug.Log("Inventory >>> +" + id);
    }

    public static void RemoveItem(string id)
    {
        items.Remove(id);
    }

    public static List<string> GetItems()
    {
        return items;
    }
}
