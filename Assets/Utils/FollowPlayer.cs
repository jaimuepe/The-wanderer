using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float smoothTime = 0.3F;

    private Transform myTransform;
    private Transform target;
    private Vector3 velocity = Vector3.zero;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        myTransform = transform;
    }

    public void ToInitialPosition()
    {
        myTransform.position = initialPosition;
    }

    public void SetTarget(GameObject target)
    {
        if (target == null)
        {
            this.target = null;
        }
        else
        {
            this.target = target.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target && target.gameObject.activeSelf)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, myTransform.position.z);
            myTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
