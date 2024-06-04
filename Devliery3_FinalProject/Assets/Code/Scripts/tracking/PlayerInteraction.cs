using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    private Collider myCollider;
    private GameObject myObject;
    private Vector3 trunkPosition1 = new Vector3(20, 11, 75); // Initial position of the trunk
    private Vector3 trunkPosition2 = new Vector3(22, 11, 75); // Initial position of the trunk for the other trash elements

    private bool isColliding = false;
    private bool isGrabbing = false;
    private float depositOffset = 2.0f; // Offset for each new Brossa deposited

    // Reference to the PluginConnector
    private PluginConnector pluginConnector;

    // Static variable to track the current object being grabbed
    private static GameObject grabbedObject = null;
    private static Dictionary<GameObject, int> objectCollisionCount = new Dictionary<GameObject, int>();

    public AudioClip grab;
    public AudioClip collect;
    public AudioClip snake;
    public AudioClip snake_mad;


    public AudioSource audioSource;

    private float timerCountdown = 1.5f;

    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    private VegetationController vegetationController;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        myObject = null;
        // Get the PluginConnector component
        pluginConnector = FindObjectOfType<PluginConnector>();

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null) // If there is no AudioSource component on the player
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component
        }

        // Store initial positions of collectible objects
        StoreInitialPositions();

        // Get the VegetationController component
        vegetationController = FindObjectOfType<VegetationController>();
    }

    void Update()
    {
        if (isColliding == true)
        {
            timerCountdown -= Time.deltaTime;

            if (timerCountdown <= 0)
            {
                timerCountdown = 0;
                isGrabbing = true;
            }
        }

        if (isGrabbing && myObject != null && myObject.CompareTag("Brossa"))
        {
            myObject.transform.position = transform.position;
        }

        // Continuously check if both players are still colliding with the grabbed object
        if (grabbedObject != null && grabbedObject.CompareTag("BrossaGran") && CountPlayersCollidingWith(grabbedObject) < 2)
        {
            ResetGrabbingState();
            Debug.Log("Continuously check if both players are still colliding with the grabbed object");

        }
        else if (grabbedObject != null)
        {
            // Keep the grabbed object at the player's position
            grabbedObject.transform.position = transform.position;
            Debug.Log("Keep the grabbed object at the player's position");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brossa") && !isGrabbing)
        {
            isColliding = true;
            myObject = other.gameObject;
            audioSource.PlayOneShot(grab);
        }

        if (other.CompareTag("BrossaGran"))
        {
            isColliding = true;
            myObject = other.gameObject;
            IncrementCollisionCount(myObject);

            // Check if two players are colliding with the object
            if (objectCollisionCount[myObject] == 2 && grabbedObject == null)
            {
                Debug.Log("Two players are colliding with BrossaGran.");
                audioSource.PlayOneShot(grab);
                isGrabbing = true; // Start grabbing the object
                grabbedObject = myObject;
            }
        }

        if (other.CompareTag("Vehicle") && isGrabbing && myObject != null)
        {

            if (myObject.CompareTag("BrossaGran"))
            {
                myObject.transform.position = trunkPosition1; // Set initial trunk position
                myObject.transform.localScale = myObject.transform.localScale / 2;

            }
            else
            {
                // If carrying Brossa and collided with the car, move Brossa to the trunk position
                myObject.transform.position = trunkPosition2; // Set initial trunk position
                trunkPosition2.z += depositOffset; // Increase z-coordinate for the next deposit
            }

            timerCountdown = 1.5f;

            audioSource.PlayOneShot(collect);
            ResetGrabbingState();

            // Decrement remaining items count
            GamestateManager.Instance.DecrementRemainingItems();

            // Update vegetation visibility based on collected items
            vegetationController.UpdateVegetationVisibility();
        }
        /*
        if (other.CompareTag("Snake") && isGrabbing && myObject != null)
        {
            audioSource.PlayOneShot(snake);

            // If colliding with the snake while grabbing an object, reset the object's position
            myObject.transform.position = initialPositions[myObject];
            ResetGrabbingState();
        }
        */
        if (other.CompareTag("Snake"))
        {
            if (isGrabbing && myObject != null)
            {
                // If colliding with the snake while grabbing an object, reset the object's position
                myObject.transform.position = initialPositions[myObject];
                audioSource.PlayOneShot(snake);
                ResetGrabbingState();
            }
            else
            {
                // Play snake sound if the player is not carrying brossa
                audioSource.PlayOneShot(snake_mad);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Brossa"))
        {
            isColliding = false;
        }
        if (other.CompareTag("BrossaGran"))
        {
            isColliding = false;
            DecrementCollisionCount(other.gameObject);

            // Check if still grabbing
            if (grabbedObject == other.gameObject && objectCollisionCount[grabbedObject] < 2)
            {
                ResetGrabbingState();
            }

            myObject = null;
        }
    }

    // Helper method to store the initial positions of collectible objects
    private void StoreInitialPositions()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Brossa");
        foreach (GameObject collectible in collectibles)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

        GameObject[] largeCollectibles = GameObject.FindGameObjectsWithTag("BrossaGran");
        foreach (GameObject largeCollectible in largeCollectibles)
        {
            initialPositions[largeCollectible] = largeCollectible.transform.position;
        }
    }

    // Helper method to check if this player is colliding with a specific object
    public bool isCollidingWith(GameObject obj)
    {
        return isColliding && myObject == obj;
    }

    // Helper method to increment the collision count for an object
    private void IncrementCollisionCount(GameObject obj)
    {
        if (objectCollisionCount.ContainsKey(obj))
        {
            objectCollisionCount[obj]++;
        }
        else
        {
            objectCollisionCount[obj] = 1;
        }
    }

    // Helper method to decrement the collision count for an object
    private void DecrementCollisionCount(GameObject obj)
    {
        if (objectCollisionCount.ContainsKey(obj))
        {
            objectCollisionCount[obj]--;
            if (objectCollisionCount[obj] <= 0)
            {
                objectCollisionCount.Remove(obj);
            }
        }
    }

    // Helper method to count how manyplayers are colliding with a specific object
    private int CountPlayersCollidingWith(GameObject obj)
    {
        int count = 0;
        if (pluginConnector != null && pluginConnector.players != null)
        {
            foreach (GameObject player in pluginConnector.players)
            {
                if (player != null && player.GetComponent<PlayerInteraction>() != null && player.GetComponent<PlayerInteraction>().isCollidingWith(obj))
                {
                    count++;
                }
            }
        }
        return count;
    }
    // Helper method to reset the grabbing state
    private void ResetGrabbingState()
    {
        isGrabbing = false; // Stop grabbing after depositing in the trunk or resetting
        grabbedObject = null; // Reset the grabbed object
        myObject = null; // Reset myObject
        isColliding = false; // Reset the colliding state
        timerCountdown = 1.5f; // Reset the timer countdown
    }

    // Helper method to check if the object is a valid collectible
    private bool IsValidCollectible(GameObject obj)
    {
        // Check if the object has one of the specified tags for collectibles
        return obj.CompareTag("Brossa");
    }
}