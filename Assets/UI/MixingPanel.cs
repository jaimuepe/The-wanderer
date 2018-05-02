using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MixingPanel : MonoBehaviour
{
    public GameObject mixingItemPrefab;
    public InventoryPanel inventoryPanel;
    public Button mixButton;
    public Button closeButton;

    public MixingSlot slot0;
    public MixingSlot slot1;
    public MixingSlot slot2;
    public ResultMixingSlot resultSlot;

    public MixingSlot currentSelectedSlot;

    public AudioClip mixingOk;
    public AudioClip mixingWrong;

    public InteractionController interactionController;
    private InteractionsDataBase interactionsDataBase;

    private void Start()
    {
        interactionsDataBase = FindObjectOfType<InteractionsDataBase>();
        gameObject.SetActive(false);

#if UNITY_ANDROID
        slot0.GetComponent<Image>().raycastTarget = true;
        slot1.GetComponent<Image>().raycastTarget = true;
        slot2.GetComponent<Image>().raycastTarget = true;
        mixButton.GetComponent<Image>().raycastTarget = true;
        closeButton.GetComponent<Image>().raycastTarget = true;
#endif

    }

    public void Show()
    {
        CharacterController2D.DisableMovement();
        interactionController.DisableInteracting();

        gameObject.SetActive(true);
        inventoryPanel.Show(InventoryState.MIXING);
        EventSystem.current.SetSelectedGameObject(slot0.gameObject);
    }

    public void Hide()
    {
        List<Item> items = GetItemsInSlots();
        for (int i = 0; i < items.Count; i++)
        {
            Inventory.AddItem(items[i].Id);
        }

        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.RestoreDefaultSprite();
        }

        currentSelectedSlot = null;

        slot0.SelectItem(null);
        slot1.SelectItem(null);
        slot2.SelectItem(null);
        ClearResultSlot();

        inventoryPanel.Hide();
        gameObject.SetActive(false);

        if (!GameTimer.timeReached)
        {
            CharacterController2D.EnableMovement();
            interactionController.EnableInteracting();
        }
    }

    public void CloseInventoryPanel(Item item)
    {
        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.RestoreDefaultSprite();
        }
        currentSelectedSlot = null;
        inventoryPanel.Hide();
    }

    public void SaveSlotAndShowInventory(MixingSlot slot)
    {
        if (Inventory.GetItems().Count == 0)
        {
            Debug.Log("NO ITEMS");
        }
        else
        {
            currentSelectedSlot = slot;
            inventoryPanel.GrabFocus();
        }
    }

    public void OnItemSelected(Item item)
    {

        ClearResultSlot();

        if (currentSelectedSlot != null)
        {
            currentSelectedSlot.RestoreDefaultSprite();

            if (item != null)
            {
                if (currentSelectedSlot.HasItem())
                {
                    Inventory.AddItem(currentSelectedSlot.GetItem().Id);
                }

                currentSelectedSlot.SelectItem(item);
                SelectNextMixingSlot();
                Inventory.RemoveItem(item.Id);
                inventoryPanel.Refresh();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(currentSelectedSlot.gameObject);
            }
        }

        currentSelectedSlot = null;
    }

    private void SelectNextMixingSlot()
    {
        if (!slot0.HasItem())
        {
            EventSystem.current.SetSelectedGameObject(slot0.gameObject);
        }
        else if (!slot1.HasItem())
        {
            EventSystem.current.SetSelectedGameObject(slot1.gameObject);
        }
        else if (!slot2.HasItem())
        {
            EventSystem.current.SetSelectedGameObject(slot2.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(mixButton.gameObject);
        }
    }

    public bool IsShowing()
    {
        return gameObject.activeSelf;
    }

    private void ClearResultSlot()
    {
        resultSlot.SetResultingItem(null);
    }

    private List<Item> GetItemsInSlots()
    {
        List<Item> items = new List<Item>();
        if (slot0.HasItem())
        {
            items.Add(slot0.GetItem());
        }
        if (slot1.HasItem())
        {
            items.Add(slot1.GetItem());
        }
        if (slot2.HasItem())
        {
            items.Add(slot2.GetItem());
        }

        return items;
    }

    public void Mix()
    {
        List<Item> items = GetItemsInSlots();
        string result = Files.GetRecipe(items);

        slot0.SelectItem(null);
        slot1.SelectItem(null);
        slot2.SelectItem(null);

        if (result != null)
        {
            resultSlot.SetResultingItem(interactionsDataBase.GetItem(result));
            Inventory.AddItem(result);

            SoundManager.Play2DClipAtPoint(mixingOk, 0.5f);
            EventSystem.current.SetSelectedGameObject(slot0.gameObject);
        }
        else if (items.Count > 0)
        {
            // No valid recipe
            for (int i = 0; i < items.Count; i++)
            {
                Inventory.AddItem(items[i].Id);
            }

            SoundManager.Play2DClipAtPoint(mixingWrong, 0.5f);
            EventSystem.current.SetSelectedGameObject(slot0.gameObject);
        }

        inventoryPanel.Refresh();
    }

    private bool CanMix()
    {
        int items = 0;
        if (slot0.HasItem())
        {
            items++;
        }
        if (slot1.HasItem())
        {
            items++;
        }
        if (slot2.HasItem())
        {
            items++;
        }

        return items > 1;
    }
}
