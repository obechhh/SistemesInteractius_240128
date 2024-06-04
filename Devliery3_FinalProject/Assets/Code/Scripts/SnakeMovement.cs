using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public List<Vector3> positions; // List of predefined positions for the snake to move to
    public float moveSpeed = 1.0f; // Speed at which the snake moves
    public float waitTime = 2.0f; // Time the snake waits before moving to the next position

    private Vector3 targetPosition; // The current target position

    void Start()
    {
        if (positions.Count > 0)
        {
            StartCoroutine(MoveToRandomPosition());
        }
    }

    IEnumerator MoveToRandomPosition()
    {
        while (true)
        {
            targetPosition = positions[Random.Range(0, positions.Count)];
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
