using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField]
    string id;

    [SerializeField]
    Sprite sprite;

    public string Id
    {
        get
        {
            return id;
        }
    }

    public Sprite GetSprite()
    {
        return sprite;
    }
}
