using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public Transform pivotPOV;
    public Transform rightPOV;
    public Transform leftPOV;
    public Transform acrossTablePOV;

    private Transform currentPOV;
    private float transitionSpeed = 0.2f; // Speed of the transition
    private bool isTransitioning = false; // Flag to check if a transition is in progress

    void Start()
    {
        currentPOV = pivotPOV;
        StartCoroutine(MoveToPOV(currentPOV));
    }

    void Update()
    {
        if (isTransitioning) return; // Ignore input if a transition is in progress

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentPOV == pivotPOV)
                StartCoroutine(MoveToPOV(rightPOV));
            else if (currentPOV == leftPOV)
                StartCoroutine(MoveToPOV(pivotPOV));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentPOV == pivotPOV)
                StartCoroutine(MoveToPOV(leftPOV));
            else if (currentPOV == rightPOV)
                StartCoroutine(MoveToPOV(pivotPOV));
        }
        else if (Input.GetKeyDown(KeyCode.W) && currentPOV == pivotPOV)
        {
            StartCoroutine(MoveToPOV(acrossTablePOV));
        }
        else if (Input.GetKeyDown(KeyCode.S) && (currentPOV == acrossTablePOV || currentPOV == rightPOV || currentPOV == leftPOV))
        {
            StartCoroutine(MoveToPOV(pivotPOV));
        }
    }

    private IEnumerator MoveToPOV(Transform newPOV)
    {
        isTransitioning = true;
        currentPOV = newPOV;
        float startTime = Time.time;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        while (Time.time < startTime + transitionSpeed)
        {
            float t = (Time.time - startTime) / transitionSpeed;
            transform.position = Vector3.Lerp(startPosition, newPOV.position, t);
            transform.rotation = Quaternion.Lerp(startRotation, newPOV.rotation, t);
            yield return null;
        }

        transform.position = newPOV.position;
        transform.rotation = newPOV.rotation;
        isTransitioning = false;
    }
}
