using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineChecker : MonoBehaviour
{

    Material mat;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void LateUpdate()
    {
        if (InteractionTrigger.currentInteraction == gameObject)
        {
            mat.SetFloat("_Outline", 1f);
        }
        else
        {
            mat.SetFloat("_Outline", 0f);
        }
    }
}
