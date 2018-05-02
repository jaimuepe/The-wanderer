using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    public CharacterController2D characterController2D;
    public InteractionController interactionController;

    public InventoryManager inventoryManager;

    private Camera mainCamera;

    public GameObject inventoryButton;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        float horizontal = 0f;
        float vertical = 0f;

#if UNITY_ANDROID

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPosition = touch.position;
            Vector2 touchPosInViewport = mainCamera.ScreenToViewportPoint(touchPosition);

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = touchPosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                continue;
            }

            float angle = Mathf.Rad2Deg * Mathf.Atan2(touchPosInViewport.y - 0.5f, touchPosInViewport.x - 0.5f);

            int roundedAngle = 45 * Mathf.RoundToInt(angle / 45f);

            int x = Mathf.RoundToInt(Mathf.Cos(Mathf.Deg2Rad * roundedAngle));
            int y = Mathf.RoundToInt(Mathf.Sin(Mathf.Deg2Rad * roundedAngle));

            if (x > 0)
            {
                horizontal += 1;
            }
            else if (x < 0)
            {
                horizontal -= 1;
            }

            if (y > 0)
            {
                vertical += 1;
            }
            else if (y < 0)
            {
                vertical -= 1;
            }
        }

#else
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
#endif

        characterController2D.RequestMove(horizontal, vertical);
    }

    private void Update()
    {

#if !UNITY_ANDROID
        if (Input.GetButtonDown("Interact"))
        {
            interactionController.Interact();
        }

        if (Input.GetButtonDown("Inventory"))
        {
            inventoryManager.ShowRecipeBookMenu();
        }
#endif
    }

    public void DisableInputs()
    {
        inventoryManager.HideMixingMenu();
        inventoryManager.HideInventory();

        CharacterController2D.DisableMovement();
        InventoryManager.DisableInventory();
        interactionController.DisableInteracting();
#if UNITY_ANDROID
        inventoryButton.SetActive(false);
#endif

    }

    public void EnableInputs()
    {
        CharacterController2D.EnableMovement();
        InventoryManager.EnableInventory();
        interactionController.EnableInteracting();

#if UNITY_ANDROID
        inventoryButton.SetActive(true);
#endif
    }
}
