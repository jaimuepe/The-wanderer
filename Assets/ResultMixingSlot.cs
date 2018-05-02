using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMixingSlot : MonoBehaviour
{
    public Item itemInSlot;
    public Image image;

    public void SetResultingItem(Item item)
    {
        itemInSlot = item;
        if (item != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = item.GetSprite();
        }
        else
        {
            image.gameObject.SetActive(false);
            image.sprite = null;
        }
    }
}
