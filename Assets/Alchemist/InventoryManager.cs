using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static bool inventoryEnabled = false;

    public MixingPanel mixingPanel;
    public InventoryPanel inventoryPanel;

    public void ShowMixingMenu()
    {
        mixingPanel.Show();
    }

    public void HideMixingMenu()
    {
        mixingPanel.Hide();
    }

    public void ShowRecipeBookMenu()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        inventoryPanel.ShowRecipeBookMenu();
    }

    public void HideInventory()
    {
        inventoryPanel.Hide();
    }

    public void ShowTradeInventory(NPC npc)
    {
        inventoryPanel.ShowTradeInventory(npc);
    }

    public static void EnableInventory()
    {
        inventoryEnabled = true;
    }

    public static void DisableInventory()
    {
        inventoryEnabled = false;
    }
}
