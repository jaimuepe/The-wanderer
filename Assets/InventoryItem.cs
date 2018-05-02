
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, ISelectHandler
{
    public Image icon;
    public Item item;
    public int index;

    private SoundManager uiSoundManager;

    public InventoryPanel inventoryPanel;

    public void OnSelect(BaseEventData eventData)
    {
        if (uiSoundManager == null)
        {
            uiSoundManager = FindObjectOfType<SoundManager>();
        }

        uiSoundManager.PlayButtonSelectedClip();

        inventoryPanel.SnapTo(index);
    }

    public void OnClick()
    {
        if (uiSoundManager == null)
        {
            uiSoundManager = FindObjectOfType<SoundManager>();
        }

        uiSoundManager.PlayButtonClickedClip();
    }
}
