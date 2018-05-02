using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public GameTimer timer;
    private InputManager inputManager;
    private FollowPlayer followPlayer;

    private void OnEnable()
    {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.EnableInputs();

        followPlayer = FindObjectOfType<FollowPlayer>();
        followPlayer.SetTarget(gameObject);

        StartCoroutine(ActivateTimerAfterDelay());
    }

    IEnumerator ActivateTimerAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        timer.gameObject.SetActive(true);
    }
}
