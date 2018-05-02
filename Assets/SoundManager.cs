using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip onButtonSelectClip;
    public AudioClip onButtonClickedClip;

    private Camera mainCamera;

    public static SoundManager instance;

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    private AudioSource mainSource;
    private AudioSource secondarySource;

    public float maxVolume;

    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        mainSource = audioSource1;
        secondarySource = audioSource2;
    }

    public void FadeOutBackgroundMusic(float fadeTime)
    {
        StartCoroutine(FadeOut(mainSource, fadeTime));
        StartCoroutine(FadeOut(secondarySource, fadeTime));
    }

    public void PlayBackgroundMusic(AudioClip backgroundMusic)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        AudioClip currentClip = mainSource.clip;
        if (currentClip == null || !currentClip.name.Equals(backgroundMusic.name))
        {
            secondarySource.clip = backgroundMusic;

            float time = currentClip == null ? 0f : fadeInTime;

            // Swap the clip
            StartCoroutine(FadeOut(mainSource, fadeOutTime));
            StartCoroutine(FadeIn(secondarySource, time));

            AudioSource aux = mainSource;
            mainSource = secondarySource;
            secondarySource = aux;

            currentClip = backgroundMusic;
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeTime = 1f)
    {
        float initialVolume = audioSource.volume;
        float volume = audioSource.volume;

        for (float t = 0f; t < 1f; t += Time.deltaTime / fadeTime)
        {
            volume = Mathf.Lerp(initialVolume, 0f, t);
            audioSource.volume = volume;
            yield return null;
        }
        audioSource.volume = 0f;
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (fadeTime == 0f)
        {
            audioSource.volume = maxVolume;
        }
        else
        {
            float initialVolume = audioSource.volume;
            float volume = audioSource.volume;
            for (float t = 0f; t < 1f; t += Time.deltaTime / fadeTime)
            {
                volume = Mathf.Lerp(initialVolume, maxVolume, t);
                audioSource.volume = volume;
                yield return null;
            }
            audioSource.volume = maxVolume;
        }
    }

    public void PlayButtonSelectedClip()
    {
        Play2DClipAtPoint(onButtonSelectClip, 0.1f);
    }

    public void PlayButtonClickedClip()
    {
        Play2DClipAtPoint(onButtonClickedClip, 0.1f);
    }

    public static void Play2DClipAtPoint(AudioClip clip, float volume)
    {
        //  Create a temporary audio source object
        GameObject tempAudioSource = new GameObject("TempAudio");

        //  Add an audio source
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

        //  Add the clip to the audio source
        audioSource.clip = clip;

        //  Set the volume
        audioSource.volume = volume;

        //  Set properties so it's 2D sound
        audioSource.spatialBlend = 0.0f;

        //  Play the audio
        audioSource.Play();

        //  Set it to self destroy
        Destroy(tempAudioSource, clip.length);
    }
}
