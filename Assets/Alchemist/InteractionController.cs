using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public bool interactingEnabled = false;
    public GameObject notifier;
    public GameObject androidNotifier;

    private InteractionTrigger trigger;

    private InteractionsDataBase interactionsDataBase;

    void Start()
    {
        interactionsDataBase = FindObjectOfType<InteractionsDataBase>();
        trigger = transform.parent.GetComponentInChildren<InteractionTrigger>();
        notifier.SetActive(false);
        androidNotifier.SetActive(false);
    }

    void Update()
    {
        if (interactingEnabled && trigger.IsInteractingWithSomething())
        {
            GameObject interaction = InteractionTrigger.currentInteraction;
            NPC npc = interaction.GetComponent<NPC>();
            if (npc && npc.IsHappy())
            {

#if UNITY_ANDROID
                androidNotifier.SetActive(false);
#else
                notifier.SetActive(false);
#endif
            }
            else
            {

#if UNITY_ANDROID
                androidNotifier.SetActive(true);
#else
                notifier.SetActive(true);
#endif
            }
        }
        else
        {

#if UNITY_ANDROID
            androidNotifier.SetActive(false);
#else
            notifier.SetActive(false);
#endif
        }
    }

    public void Interact()
    {
        if (interactingEnabled && trigger.IsInteractingWithSomething())
        {
            GameObject interaction = InteractionTrigger.currentInteraction;
            interactionsDataBase.Interact(interaction);
        }
    }

    public void EnableInteracting()
    {
        StartCoroutine(EnableInteractingNextFrame());
    }

    IEnumerator EnableInteractingNextFrame()
    {
        yield return new WaitForEndOfFrame();
        interactingEnabled = true;
    }

    public void DisableInteracting()
    {
        interactingEnabled = false;
    }
}
