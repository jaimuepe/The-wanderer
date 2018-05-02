using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpdateDepth : MonoBehaviour
{
    public bool isDynamic;
    public float extraOffset = 0f;

    SpriteRenderer sr;
    PolygonCollider2D polygonCollider;

    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (polygonCollider)
        {
            sr.sortingOrder = -(int)(extraOffset + 100 * polygonCollider.bounds.center.y);
        }
    }

    void Update()
    {
        if (isDynamic)
        {
            sr.sortingOrder = -(int)(extraOffset + 100 * polygonCollider.bounds.center.y);
        }
    }
}
