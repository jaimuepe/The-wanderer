using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    public Clock clock;
    public Image fadePanel;
    public float fadeOutTime;

    public GameObject player;

    public static bool timeReached = false;

    public float gameTimeInSeconds;

    public GameObject outro;

    private float accumulatedGameTimeGameScaleInSeconds = 0f;

    private float totalTimeInGameScale = 6 * 60 * 60;

    private int previousFullMinute = -1;

    private void Update()
    {
        float secondsSinceLastFrame = Time.deltaTime;
        accumulatedGameTimeGameScaleInSeconds += (secondsSinceLastFrame * totalTimeInGameScale / gameTimeInSeconds);

        int minutes = (int)(Mathf.Floor(accumulatedGameTimeGameScaleInSeconds)) / 60;

        if (minutes != previousFullMinute)
        {
            previousFullMinute = minutes;

            if (!timeReached && accumulatedGameTimeGameScaleInSeconds >= totalTimeInGameScale)
            {
                timeReached = true;
                EndGame();
            }

            if (clock.gameObject.activeSelf)
            {
                clock.UpdateClock(minutes);
            }
        }
    }

    public void ForceGameEnd()
    {
        accumulatedGameTimeGameScaleInSeconds = totalTimeInGameScale;
    }

    private void EndGame()
    {
        InputManager inputManager = FindObjectOfType<InputManager>();
        inputManager.DisableInputs();
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        yield return new WaitForSeconds(3f);
        SoundManager.instance.FadeOutBackgroundMusic(fadeOutTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
        fadePanel.gameObject.SetActive(true);

        float initialAlpha = 0f;
        float alpha = 0f;

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeOutTime)
        {
            alpha = Mathf.Lerp(initialAlpha, 1f, t);
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, alpha);
            yield return null;
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1f);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Clean());
    }

    IEnumerator Clean()
    {
        yield return new WaitForSeconds(2f);

        player.gameObject.SetActive(false);
        Camera mainCamera = Camera.main;

        FollowPlayer followPlayer = mainCamera.GetComponent<FollowPlayer>();
        followPlayer.SetTarget(null);
        followPlayer.ToInitialPosition();

        clock.gameObject.SetActive(false);
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        outro.gameObject.SetActive(true);

        float initialAlpha = 1f;
        float alpha = 1f;

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeOutTime)
        {
            alpha = Mathf.Lerp(initialAlpha, 0f, t);
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
    }
}
