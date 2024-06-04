using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Recycle : MonoBehaviour
{
    private GameObject myObject;
    private Collider myCollider;

    private bool isColliding = false;
    private bool isGrabbing = false;
   
    private float timerCountdown = 1.5f;

    // Reference to the PluginConnector
    private PluginConnector pluginConnector;

    public AudioClip miss;
    public AudioClip grab;
    public AudioClip collect;
    public AudioSource audioSource;

    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();
    private static GameObject grabbedObject = null;

    private static Dictionary<GameObject, int> objectCollisionCount = new Dictionary<GameObject, int>();

    void Start()
    {
        myCollider = GetComponent<Collider>();
        myObject = null;
        //set gamestatemanager remaining items to equal total items
        GamestateManager.Instance.remainingItems = GamestateManager.Instance.totalItems;

        // Get the PluginConnector component
        pluginConnector = FindObjectOfType<PluginConnector>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null) // If there is no AudioSource component on the sheep
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component
        }
        StoreInitialPositions();
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

        if (isGrabbing && !myObject.CompareTag("BrossaGran") && myObject != null)
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

        /*
        if (isGrabbing)
        {
            myObject.transform.position = transform.position;
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbing && (other.CompareTag("EnvasosLleugers") || other.CompareTag("PaperCartró") || other.CompareTag("Orgànic") || other.CompareTag("Vidre")))
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

        if (other.CompareTag("planta_rec") && isGrabbing && myObject != null)
        {
            
            if (myObject.CompareTag("BrossaGran"))
            {
                Destroy(myObject);
                audioSource.PlayOneShot(collect);
                GamestateManager.Instance.DecrementRemainingItems();

            }
            timerCountdown = 1.5f;
            ResetGrabbingState();
            
        }


            if ((other.CompareTag("ContenidorEnvasosLleugers") && myObject.CompareTag("EnvasosLleugers")) || (other.CompareTag("ContenidorPaperCartró") && myObject.CompareTag("PaperCartró")) || (other.CompareTag("ContenidorOrgànic") && myObject.CompareTag("Orgànic")) || (other.CompareTag("ContenidorVidre") && myObject.CompareTag("Vidre")))
            {
                Destroy(myObject);
                isGrabbing = false;
                isColliding = false;
                audioSource.PlayOneShot(collect);
                timerCountdown = 1.5f;
                GamestateManager.Instance.DecrementRemainingItems();

        }
        else
            {
                if (other.CompareTag("ContenidorEnvasosLleugers") || other.CompareTag("ContenidorPaperCartró") || other.CompareTag("ContenidorOrgànic") || other.CompareTag("ContenidorVidre"))
                {
                    isGrabbing = false;
                    isColliding = false;
                    myObject.transform.position = initialPositions[myObject];
                    audioSource.PlayOneShot(miss);
                    timerCountdown = 1.5f;
                }
            }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("EnvasosLleugers") || other.CompareTag("PaperCartró") || other.CompareTag("Orgànic") || other.CompareTag("Vidre")))
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

    private void StoreInitialPositions()
    {
        GameObject[] collectiblesLleugers = GameObject.FindGameObjectsWithTag("EnvasosLleugers");
        foreach (GameObject collectible in collectiblesLleugers)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

        GameObject[] collectiblesPaper = GameObject.FindGameObjectsWithTag("PaperCartró");
        foreach (GameObject collectible in collectiblesPaper)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

        GameObject[] collectiblesOrganic = GameObject.FindGameObjectsWithTag("Orgànic");
        foreach (GameObject collectible in collectiblesOrganic)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

        GameObject[] collectiblesVidre = GameObject.FindGameObjectsWithTag("Vidre");
        foreach (GameObject collectible in collectiblesVidre)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

        //now for washing machine
        GameObject[] largeCollectibles = GameObject.FindGameObjectsWithTag("BrossaGran");
        foreach (GameObject collectible in largeCollectibles)
        {
            initialPositions[collectible] = collectible.transform.position;
        }

    }

    // Helper method to check if this player is colliding with a specific object
    private bool IsCollidingWith(GameObject obj)
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
                if (player != null && player.GetComponent<Recycle>() != null && player.GetComponent<Recycle>().IsCollidingWith(obj))
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
}
