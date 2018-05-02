using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum Quadrant
{
    NORTHEAST, SOUTHEAST, SOUTHWEST, NORTHWEST
};

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField]
    private static bool movementEnabled = true;

    [SerializeField]
    private float movementSpeed = 3f;

    public AudioClip steps;

    public float stepsDelay;
    private float stepsCounter;

    private Transform myTransform;
    private Rigidbody2D rb;
    private Animator animator;

    private InteractionTrigger interactionTrigger;
    private Quadrant lastValidMovementQuadrant = Quadrant.SOUTHWEST;

    private bool isMoving = false;

    private void Awake()
    {
        myTransform = transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        interactionTrigger = GetComponentInChildren<InteractionTrigger>();
    }

    private float xMovement;
    private float yMovement;

    private void Update()
    {
        if (isMoving)
        {
            stepsCounter -= Time.deltaTime;
            if (stepsCounter <= 0)
            {
                SoundManager.Play2DClipAtPoint(steps, 0.15f);
                stepsCounter = stepsDelay;
            }
        }
        else
        {
            stepsCounter = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = Vector2.zero;
        if (Mathf.Abs(xMovement) > 0.001f || Mathf.Abs(yMovement) > 0.001f)
        {
            isMoving = true;
            velocity = Move(xMovement, yMovement);
            UpdateLastValidMovementQuadrant(velocity);
        }
        else
        {
            isMoving = false;
        }
        PlayAnimationClip(velocity);

        xMovement = 0f;
        yMovement = 0f;
    }

    public void RequestMove(float horizontal, float vertical)
    {
        if (movementEnabled)
        {
            xMovement = horizontal;
            yMovement = vertical;
        }
    }

    private void UpdateLastValidMovementQuadrant(Vector2 velocity)
    {
        if (velocity.y == 0)
        {
            if (lastValidMovementQuadrant == Quadrant.NORTHEAST || lastValidMovementQuadrant == Quadrant.NORTHWEST)
            {
                if (velocity.x > 0)
                {
                    lastValidMovementQuadrant = Quadrant.NORTHEAST;
                }
                else
                {
                    lastValidMovementQuadrant = Quadrant.NORTHWEST;
                }
            }
            else
            {
                if (velocity.x > 0)
                {
                    lastValidMovementQuadrant = Quadrant.SOUTHEAST;
                }
                else
                {
                    lastValidMovementQuadrant = Quadrant.SOUTHWEST;
                }
            }
        }
        else if (velocity.x == 0)
        {
            if (lastValidMovementQuadrant == Quadrant.NORTHEAST || lastValidMovementQuadrant == Quadrant.SOUTHEAST)
            {
                if (velocity.y > 0)
                {
                    lastValidMovementQuadrant = Quadrant.NORTHEAST;
                }
                else
                {
                    lastValidMovementQuadrant = Quadrant.SOUTHEAST;
                }
            }
            else
            {
                if (velocity.y > 0)
                {
                    lastValidMovementQuadrant = Quadrant.NORTHWEST;
                }
                else
                {
                    lastValidMovementQuadrant = Quadrant.SOUTHWEST;
                }
            }
        }
        else if (velocity.x > 0 && velocity.y > 0)
        {
            // 1st quad
            lastValidMovementQuadrant = Quadrant.NORTHEAST;
        }
        else if (velocity.x > 0)
        {
            // 2nd quad
            lastValidMovementQuadrant = Quadrant.SOUTHEAST;
        }
        else if (velocity.y < 0)
        {
            // 3rd quad
            lastValidMovementQuadrant = Quadrant.SOUTHWEST;
        }
        else
        {
            // 4th quad
            lastValidMovementQuadrant = Quadrant.NORTHWEST;
        }

        interactionTrigger.UpdateTriggerPosition(lastValidMovementQuadrant);
    }

    private Vector2 Move(float horizontal, float vertical)
    {
        Vector2 velocity = Time.deltaTime * movementSpeed * new Vector2(horizontal, vertical).normalized;

        rb.velocity = velocity;
        return velocity;
    }

    private string DecideIdleAnimationClip()
    {

        StringBuilder clipName = new StringBuilder();
        clipName.Append("idle_");

        switch (lastValidMovementQuadrant)
        {
            case Quadrant.NORTHEAST:
                clipName.Append("northeast");
                break;
            case Quadrant.SOUTHEAST:
                clipName.Append("southeast");
                break;
            case Quadrant.SOUTHWEST:
                clipName.Append("southwest");
                break;
            case Quadrant.NORTHWEST:
                clipName.Append("northwest");
                break;
        }

        return clipName.ToString();
    }

    private string DecideWalkAnimationClip(Vector2 velocity)
    {
        StringBuilder clipName = new StringBuilder();
        clipName.Append("walk_");

        switch (lastValidMovementQuadrant)
        {
            case Quadrant.NORTHEAST:
                clipName.Append("northeast");
                break;
            case Quadrant.SOUTHEAST:
                clipName.Append("southeast");
                break;
            case Quadrant.SOUTHWEST:
                clipName.Append("southwest");
                break;
            case Quadrant.NORTHWEST:
                clipName.Append("northwest");
                break;
        }

        return clipName.ToString();
    }

    private void PlayAnimationClip(Vector2 velocity)
    {
        if (!animator)
        {
            return;
        }

        string clipName;

        if (velocity != Vector2.zero)
        {
            clipName = DecideWalkAnimationClip(velocity);
        }
        else
        {
            clipName = DecideIdleAnimationClip();
        }

        animator.Play(clipName);
    }

    public static void EnableMovement()
    {
        movementEnabled = true;
    }

    public static void DisableMovement()
    {
        movementEnabled = false;
    }
}
