using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class InteractionsDataBase : MonoBehaviour
{

    public AudioClip pickupClip;

    private InventoryManager inventoryManager;

    public List<Item> itemPool;

    private Dictionary<string, Item> items;

    private void Start()
    {
        items = new Dictionary<string, Item>();
        for (int i = 0; i < itemPool.Count; i++)
        {
            items.Add(itemPool[i].Id, itemPool[i]);
        }
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public Item GetItem(string id)
    {
        VerifyItemId(id);
        return items[id];
    }

    private void VerifyItemId(string id)
    {
        Assert.IsTrue(items.ContainsKey(id));
    }

    public void Interact(GameObject interaction)
    {
        string interactionTag = interaction.tag;

        switch (interactionTag)
        {
            case "Cauldron":
                inventoryManager.ShowMixingMenu();
                break;
            case "apple":
            case "goo":
            case "spooky_shroom":
            case "spiky_flower":
            case "shroom":
            case "shroom2":
                VerifyItemId(interactionTag);
                Inventory.AddItem(interactionTag);
                SoundManager.Play2DClipAtPoint(pickupClip, 0.2f);
                Destroy(interaction);
                break;
            case "npc01":
            case "npc_toilet":
            case "npc_cornfield":
            case "npc_half_guy":
            case "npc_dead_guy":
            case "npc_kid":
            case "npc_frog_guy":
                NPC npc = interaction.GetComponent<NPC>();

                SoundManager.Play2DClipAtPoint(npc.voice, 0.2f);
                if (npc.IsSad())
                {
                    inventoryManager.ShowTradeInventory(npc);
                }
                break;
            default:
                Debug.Log("Interaction with " + interactionTag + " not defined");
                break;
        }
    }
}
