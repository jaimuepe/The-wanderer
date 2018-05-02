using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum InventoryState
{
    LOOKING, MIXING, TRADING
};

public class InventoryPanel : MonoBehaviour
{
    public RectTransform scrollRect;
    public RectTransform contentPanel;

    public InventoryItem inventoryItemPrefab;
    public GridLayoutGroup gridLayoutGroup;

    public MixingPanel mixingPanel;
    public RecipeBook recipeBook;
    public Clock clock;

    public InteractionsDataBase interactionsDataBase;
    public InteractionController interactionController;

    public GameObject particleSystemSuccessTemplate;

    private Button firstButton;

    public InventoryState state;

    private NPC tradingWidth = null;

    private void Start()
    {
#if UNITY_ANDROID
        GetComponent<ScrollRect>().horizontal = true;
#endif
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        Show(state);
    }

    public void Show(InventoryState state)
    {
        this.state = state;

        foreach (Transform children in contentPanel.transform)
        {
            Destroy(children.gameObject);
        }

        CharacterController2D.DisableMovement();
        interactionController.DisableInteracting();

        firstButton = null;
        List<string> items = Inventory.GetItems();
        items.Sort();

        List<Button> buttonList = new List<Button>();

        for (int i = 0; i < items.Count; i++)
        {
            string id = items[i];
            Item item = interactionsDataBase.GetItem(id);

            InventoryItem inventoryItem = Instantiate(inventoryItemPrefab);

#if UNITY_ANDROID
            inventoryItem.GetComponent<Image>().raycastTarget = true;
#endif

            inventoryItem.item = item;
            inventoryItem.icon.sprite = item.GetSprite();

            inventoryItem.inventoryPanel = this;
            inventoryItem.index = i;

            inventoryItem.gameObject.name = "slot_" + id;
            Button btInventoryItem = inventoryItem.GetComponent<Button>();

#if !UNITY_ANDROID
            Navigation navigation = btInventoryItem.navigation;
            navigation.mode = Navigation.Mode.Explicit;
#endif

            btInventoryItem.onClick.AddListener(
                delegate
                {
                    OnItemSelected(inventoryItem.item);
                });

            inventoryItem.transform.SetParent(contentPanel.transform, false);

            buttonList.Add(btInventoryItem);
        }

#if !UNITY_ANDROID
        for (int i = 0; i < buttonList.Count; i++)
        {
            Button bt = buttonList[i];

            Navigation navigation = bt.navigation;

            navigation.mode = Navigation.Mode.Explicit;
            if (i == 0)
            {
                navigation.selectOnLeft = buttonList[buttonList.Count - 1];
                if (buttonList.Count == 1)
                {
                    navigation.selectOnRight = buttonList[0];
                }
                else
                {
                    navigation.selectOnRight = buttonList[i + 1];
                }
            }
            else if (i == buttonList.Count - 1)
            {
                navigation.selectOnLeft = buttonList[i - 1];
                navigation.selectOnRight = buttonList[0];
            }
            else
            {
                navigation.selectOnLeft = buttonList[i - 1];
                navigation.selectOnRight = buttonList[i + 1];
            }

            bt.navigation = navigation;
        }

#endif

        gameObject.SetActive(true);

        if (buttonList.Count > 0)
        {
            firstButton = buttonList[0];
        }

        if (state == InventoryState.LOOKING)
        {
            recipeBook.Show();
            clock.Show();
        }
        else
        {
            recipeBook.Hide();
        }
    }

    public void GrabFocus()
    {
#if !UNITY_ANDROID
        if (firstButton)
        {
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
#endif
    }

    public void Hide()
    {
        tradingWidth = null;
        if (state == InventoryState.LOOKING)
        {
            recipeBook.Hide();

            if (!GameTimer.timeReached)
            {
                clock.Hide();
            }
        }

        gameObject.SetActive(false);
    }

    public void HideAndRestoreMovement()
    {
        Hide();

        if (!GameTimer.timeReached)
        {
            CharacterController2D.EnableMovement();
            interactionController.EnableInteracting();
        }
    }

    public bool IsShowing()
    {
        return gameObject.activeSelf;
    }

    public void SnapTo(int index)
    {
        Canvas.ForceUpdateCanvases();

        int maxCellsPerRow = GetMaxCellsPerRow();

        if (index >= maxCellsPerRow)
        {
            int focusInCell = index - maxCellsPerRow + 1;

            float slotWidth = gridLayoutGroup.cellSize.x;
            float paddingLeft = gridLayoutGroup.padding.left;
            float spacingWidth = gridLayoutGroup.spacing.x;

            contentPanel.anchoredPosition = -new Vector2(
                paddingLeft + focusInCell * slotWidth + (focusInCell - 1) * spacingWidth,
                0);
        }
        else
        {
            contentPanel.anchoredPosition = Vector2.zero;
        }
    }

    private int GetMaxCellsPerRow()
    {
        float slotWidth = gridLayoutGroup.cellSize.x;

        float paddingLeft = gridLayoutGroup.padding.left;

        float viewportWidth = GetComponent<RectTransform>().sizeDelta.x;
        return (int)Mathf.Floor((viewportWidth - paddingLeft) / slotWidth) - 1;
    }

    public void ShowRecipeBookMenu()
    {
        if (!mixingPanel.IsShowing())
        {
            if (IsShowing())
            {
                HideAndRestoreMovement();
            }
            else
            {
                if (InventoryManager.inventoryEnabled)
                {
                    Show(InventoryState.LOOKING);
                }
            }
        }
        else
        {
            mixingPanel.Hide();
        }
    }

    public void ShowTradeInventory(NPC npc)
    {
        Show(InventoryState.TRADING);
        tradingWidth = npc;
        GrabFocus();
    }

    public AudioClip successClip;
    public GameTimer gameTimer;

    public void OnItemSelected(Item item)
    {
        if (state == InventoryState.MIXING)
        {
            mixingPanel.OnItemSelected(item);
        }
        else if (state == InventoryState.TRADING)
        {
            if (Files.CheckNpcItem(tradingWidth.tag, item.Id))
            {
                Inventory.RemoveItem(item.Id);
                tradingWidth.BeHappy();

                tradingWidth.Transform();

                GameObject particleSystem = Instantiate(particleSystemSuccessTemplate);
                particleSystem.transform.SetParent(tradingWidth.transform, false);
                particleSystem.SetActive(true);
                Destroy(particleSystem, 2f);

                HideAndRestoreMovement();

                SoundManager.Play2DClipAtPoint(successClip, 0.3f);

                bool forceEndGame = true;
                NPC[] npcs = FindObjectsOfType<NPC>();
                for (int i = 0; i < npcs.Length; i++)
                {
                    if (npcs[i].IsSad())
                    {
                        forceEndGame = false;
                        break;
                    }
                }

                if (forceEndGame)
                {
                    gameTimer.ForceGameEnd();
                }
            }
            else
            {
                Debug.Log("I don't want that!");
                HideAndRestoreMovement();
            }
        }
    }
}
