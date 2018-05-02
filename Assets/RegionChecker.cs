using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionChecker : MonoBehaviour
{

    public LayerMask mask;

    PolygonCollider2D polygonCollider;

    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        ContactFilter2D filter = new ContactFilter2D()
        {
            layerMask = mask,
            useTriggers = true
        };

        Collider2D[] results = new Collider2D[4];
        int ammount = Physics2D.OverlapCollider(polygonCollider, filter, results);

        if (ammount == 1)
        {
            Collider2D collider = results[0];
            Region region = collider.GetComponent<Region>();
            SoundManager.instance.PlayBackgroundMusic(region.backgroundMusic);
        }
    }
}
