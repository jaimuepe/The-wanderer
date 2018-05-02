using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour, IPointerClickHandler
{
    public AudioClip pagesClip;

    public List<Sprite> pages;

    int currentPage = 0;
    int maxPages;

    public Image background;
    public Image image;

    private void Start()
    {
        maxPages = pages.Count;
        gameObject.SetActive(false);
    }

    private void Update()
    {
#if !UNITY_ANDROID
        if (Input.GetButtonDown("Interact"))
        {
            NextPage(true);
        }
#endif
    }

    public void Show()
    {
        gameObject.SetActive(true);
        currentPage--;
        NextPage(false);
    }

    public bool IsShowing()
    {
        return gameObject.activeSelf;
    }

    public void Hide()
    {
        if (fadeOutFadeInCoroutine != null)
        {
            StopCoroutine(fadeOutFadeInCoroutine);
            fadeOutFadeInCoroutine = null;
        }

        gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }

    public void NextPage(bool delay)
    {
        if (currentPage == maxPages - 1)
        {
            currentPage = 0;
        }
        else
        {
            currentPage += 1;
        }

        SoundManager.Play2DClipAtPoint(pagesClip, 0.2f);
        RestartCoroutine(delay);
    }

    private void RestartCoroutine(bool delay)
    {
        if (fadeOutFadeInCoroutine != null)
        {
            StopCoroutine(fadeOutFadeInCoroutine);
        }
        fadeOutFadeInCoroutine = StartCoroutine(FadeOutFadeIn(delay));
    }

    Coroutine fadeOutFadeInCoroutine;

    IEnumerator FadeOutFadeIn(bool delay)
    {

        float initialAlpha = background.color.a;
        float alpha = initialAlpha;

        if (delay)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

            for (float t = 0f; t < 1f; t += Time.deltaTime / 0.3f)
            {
                alpha = Mathf.Lerp(initialAlpha, 0f, t);
                Debug.Log("Fadeout a = " + alpha);
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }

        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        image.sprite = pages[currentPage];

        if (delay)
        {
            initialAlpha = 0f;
            alpha = 0f;

            for (float t = 0f; t < 1f; t += Time.deltaTime / 0.3f)
            {
                alpha = Mathf.Lerp(initialAlpha, 1f, t);
                Debug.Log("Fadein a = " + alpha);
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                yield return null;
            }
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
#if UNITY_ANDROID
        NextPage(true);
#endif
    }
}
