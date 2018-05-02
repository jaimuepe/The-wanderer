using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool isHappy = false;

    public Sprite happySprite;

    public AudioClip voice;

    public GameObject spawnGameObject;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public bool IsHappy()
    {
        return isHappy;
    }

    public bool IsSad()
    {
        return !isHappy;
    }

    public void BeHappy()
    {
        isHappy = true;
    }

    public float colliderOffsetY;
    public float offsetY;


    public void Transform()
    {
        StartCoroutine(TransformCoroutine());
    }

    IEnumerator TransformCoroutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (CompareTag("npc_toilet"))
        {
            spawnGameObject.gameObject.SetActive(true);
            gameObject.layer = 0;
        }
        else
        {
            transform.position = transform.position + new Vector3(0f, offsetY, 0f);
            Collider2D collider = GetComponent<Collider2D>();
            collider.offset -= new Vector2(0f, colliderOffsetY);
            sr.sprite = happySprite;
        }
    }
}
