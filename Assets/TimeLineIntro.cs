using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineIntro : MonoBehaviour
{

    PlayableDirector director;
    InputManager inputManager;

    // Use this for initialization
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        director = GetComponent<PlayableDirector>();
        float duration = (float)director.duration;

        StartCoroutine(EnableInputAfterIntro(duration));
    }

    IEnumerator EnableInputAfterIntro(float duration)
    {
        yield return new WaitForSeconds(duration);
        director.Stop();
        inputManager.EnableInputs();
    }
}
