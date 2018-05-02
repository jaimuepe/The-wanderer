using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoMouse : MonoBehaviour
{

    GameObject lastSelectedGameObject;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
        }
        else
        {
            lastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        }
    }
}
