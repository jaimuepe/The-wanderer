using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public Image image;
    public GameObject shortHand;
    public GameObject longHand;

    public Image shortHandImage;
    public Image longHandImage;

    public float fadeTime;
    public float stayTime;

    public InventoryPanel inventoryPanel;

    public int notifyEveryNHours = 1;

    private int lastNotifiedHour = -1;
    private bool visible = false;

    private void Start()
    {
        Hide();
    }

    public void Show()
    {
        StopFadeCoroutines();
        SetAlpha(1f);
        visible = true;

        if (clockSoundsCoroutine == null)
        {
            clockSoundsCoroutine = StartCoroutine(TickTock());
        }
    }

    private void StopFadeCoroutines()
    {
        if (FadeOutCoroutine != null)
        {
            StopCoroutine(FadeOutCoroutine);
            FadeOutCoroutine = null;
        }
        if (FadeInCoroutine != null)
        {
            StopCoroutine(FadeInCoroutine);
            FadeInCoroutine = null;
        }
    }

    private void StopCoroutines()
    {
        StopFadeCoroutines();

        if (clockSoundsCoroutine != null)
        {
            StopCoroutine(clockSoundsCoroutine);
            clockSoundsCoroutine = null;
        }
    }

    public void Hide()
    {
        StopCoroutines();
        SetAlpha(0f);
        visible = false;
    }

    public void UpdateClock(int minutes)
    {
        int min = minutes % 60;
        int hours = minutes / 60;

        float angleLongHand = min * 360 / 60f;
        longHand.transform.localEulerAngles = new Vector3(0f, 0f, -angleLongHand);

        float angleShortHand = hours * 360 / 12f;
        shortHand.transform.localEulerAngles = new Vector3(0f, 0f, -angleShortHand);

        if (hours != lastNotifiedHour)
        {

            lastNotifiedHour = hours;
            if (!visible)
            {
                StopCoroutines();
                FadeInCoroutine = StartCoroutine(FadeIn());
            }
        }
    }

    Coroutine FadeInCoroutine;

    IEnumerator FadeIn()
    {
        if (!visible)
        {
            if (FadeOutCoroutine != null)
            {
                StopCoroutine(FadeOutCoroutine);
                FadeOutCoroutine = null;
            }

            if (clockSoundsCoroutine == null)
            {
                clockSoundsCoroutine = StartCoroutine(TickTock());
            }

            float initialAlpha = 0f;
            float alpha = 0f;
            SetAlpha(0f);

            for (float t = 0f; t < 1f; t += Time.deltaTime / fadeTime)
            {
                alpha = Mathf.Lerp(initialAlpha, 1f, t);
                SetAlpha(alpha);
                yield return null;
            }
            SetAlpha(1f);
        }

        visible = true;

        if (!GameTimer.timeReached)
        {
            yield return new WaitForSeconds(stayTime);
            FadeOutCoroutine = StartCoroutine(FadeOut());
        }
    }

    Coroutine FadeOutCoroutine;

    private void SetAlpha(float alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        shortHandImage.color = new Color(shortHandImage.color.r, shortHandImage.color.g, shortHandImage.color.b, alpha);
        longHandImage.color = new Color(longHandImage.color.r, longHandImage.color.g, longHandImage.color.b, alpha);
    }

    IEnumerator FadeOut()
    {
        if (!inventoryPanel.IsShowing() || inventoryPanel.state != InventoryState.LOOKING)
        {

            if (FadeInCoroutine != null)
            {
                StopCoroutine(FadeInCoroutine);
                FadeInCoroutine = null;
            }

            float initialAlpha = 1f;
            float alpha = 1f;
            SetAlpha(1f);

            for (float t = 0f; t < 1f; t += Time.deltaTime / fadeTime)
            {
                alpha = Mathf.Lerp(initialAlpha, 0f, t);
                SetAlpha(alpha);
                yield return null;
            }
            Hide();
        }
    }

    public AudioClip tick;
    public AudioClip tock;

    private Coroutine clockSoundsCoroutine;

    private IEnumerator TickTock()
    {
        while (true)
        {
            SoundManager.Play2DClipAtPoint(tick, 0.05f);
            yield return new WaitForSeconds(1f);
            SoundManager.Play2DClipAtPoint(tock, 0.05f);
            yield return new WaitForSeconds(1f);
        }
    }
}
