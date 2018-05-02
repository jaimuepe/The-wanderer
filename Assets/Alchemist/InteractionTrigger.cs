using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    public PolygonCollider2D northEastPosition;
    public PolygonCollider2D southEastPosition;
    public PolygonCollider2D southWestPosition;
    public PolygonCollider2D northWestPosition;

    public LayerMask triggerMask;

    public static GameObject currentInteraction;

    private InteractionController interactionController;

    private void Start()
    {
        interactionController = FindObjectOfType<InteractionController>();
    }

    public void UpdateTriggerPosition(Quadrant newQuadrant)
    {
        Vector2 newOffset = Vector2.zero;

        northEastPosition.gameObject.SetActive(false);
        southEastPosition.gameObject.SetActive(false);
        southWestPosition.gameObject.SetActive(false);
        northWestPosition.gameObject.SetActive(false);

        switch (newQuadrant)
        {
            case Quadrant.NORTHEAST:
                northEastPosition.gameObject.SetActive(true);
                break;
            case Quadrant.SOUTHEAST:
                southEastPosition.gameObject.SetActive(true);
                break;
            case Quadrant.SOUTHWEST:
                southWestPosition.gameObject.SetActive(true);
                break;
            case Quadrant.NORTHWEST:
                northWestPosition.gameObject.SetActive(true);
                break;
        }
    }

    public bool IsInteractingWithSomething()
    {
        return currentInteraction != null;
    }

    private PolygonCollider2D GetActiveCollider()
    {
        if (northEastPosition.gameObject.activeSelf)
        {
            return northEastPosition;
        }
        if (southEastPosition.gameObject.activeSelf)
        {
            return southEastPosition;
        }
        if (southWestPosition.gameObject.activeSelf)
        {
            return southWestPosition;
        }
        return northWestPosition;
    }

    private void Update()
    {
        
        if (!interactionController.interactingEnabled)
        {
            currentInteraction = null;
            return;
        }

        ContactFilter2D filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = triggerMask
        };

        PolygonCollider2D polygonCollider = GetActiveCollider();
        Collider2D[] colliders = new Collider2D[5];
        int ammount = polygonCollider.OverlapCollider(filter, colliders);
        if (ammount > 0)
        {
            currentInteraction = colliders[0].gameObject;
        } else
        {
            currentInteraction = null;
        }
    }
}
