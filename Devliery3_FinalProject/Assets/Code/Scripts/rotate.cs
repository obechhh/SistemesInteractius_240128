using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float tiltAngle = 10f; // Maximum tilt angle
    public float tiltSpeed = 50f; // Speed of tilt
    public float moveDistance = 0.1f; // Distance to move back and forth
    public float moveSpeed = 0.5f; // Speed of movement
    public float minStopDuration = 0.1f; // Minimum duration wind stops blowing (in seconds)
    public float maxStopDuration = 1f; // Maximum duration wind stops blowing (in seconds)

    private bool movingForward = true; // Flag to track movement direction
    private Vector3 originalPosition; // Original position of the tree

    void Start()
    {
        // Save the original position of the tree
        originalPosition = transform.position;

        // Start the wind effect coroutine
        StartCoroutine(WindEffect());
    }

    void Update()
    {
        // Calculate the target rotation based on movement direction
        float targetRotation = movingForward ? tiltAngle : -tiltAngle;

        // Tilt the tree around its local X-axis
        float currentRotation = Mathf.MoveTowardsAngle(transform.localEulerAngles.x, targetRotation, tiltSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(currentRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    IEnumerator WindEffect()
    {
        while (true)
        {
            // Move the tree back and forth along its Z-axis
            float targetZ = movingForward ? originalPosition.z + moveDistance : originalPosition.z - moveDistance;
            Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y, targetZ);

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Change direction
            movingForward = !movingForward;

            // Wait for a while before starting the next movement
            yield return new WaitForSeconds(Random.Range(minStopDuration, maxStopDuration));
        }
    }
}