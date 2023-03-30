using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProp : MonoBehaviour
{
    public Transform alternateLocation;
    public float totalMovementTime;
    
    private Vector3 origin;
    private float currentMovementTime;
    private bool hasMoved = false;

    void Awake()
    {
        origin = transform.localPosition;
    }

    public void Move()
    {
        if (!hasMoved) {
            hasMoved = true;
            StartCoroutine(moveObject());
        }
    }

    private IEnumerator moveObject()
    {
        while (Vector3.Distance(transform.localPosition, alternateLocation.localPosition) > 0) {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(origin, alternateLocation.localPosition, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }
}