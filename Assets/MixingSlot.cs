using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MixingSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{

    public Sprite selectedSprite;
    public Sprite defaultSprite;

    public int slotIndex;
    private Item itemInSlot = null;

    public Image slotBg;
    public Image image;

    private MixingPanel mixingPanel;

    private void Start()
    {
        mixingPanel = FindObjectOfType<MixingPanel>();
    }

    public void SelectItem(Item item)
    {
        itemInSlot = item;
        if (item != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = item.GetSprite();
        }
        else
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }
    }

    public void RestoreDefaultSprite()
    {
        slotBg.sprite = defaultSprite;
    }

    public bool HasItem()
    {
        return itemInSlot != null;
    }

    public Item GetItem()
    {
        return itemInSlot;
    }

    public void OnSelect(BaseEventData eventData)
    {
#if !UNITY_ANDROID
        slotBg.sprite = selectedSprite;
#endif
    }

    public void OnDeselect(BaseEventData eventData)
    {
#if !UNITY_ANDROID
        if (this != mixingPanel.currentSelectedSlot)
        {
            slotBg.sprite = defaultSprite;
        }
#endif
    }
}
