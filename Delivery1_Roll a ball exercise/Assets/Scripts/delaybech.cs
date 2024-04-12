using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStart : MonoBehaviour
{
    public float delayTime = 3f; // Adjust this to set the delay time before the game starts

    void Start()
    {
        StartCoroutine(StartGameAfterDelay());
    }

    IEnumerator StartGameAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        // Code to start the game goes here
        // For example, you could enable player controls or trigger some initial action
    }
}
