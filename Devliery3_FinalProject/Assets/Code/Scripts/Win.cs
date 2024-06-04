using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win: MonoBehaviour
{
    private float timerCountdown = 1.5f;
    private bool isColliding = false;
    private Collider myCollider;
    private GameObject myObject;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding == true)
        {
            timerCountdown -= Time.deltaTime;

            if (timerCountdown <= 0)
            {
                timerCountdown = 0;
                LoadScene("Inici");

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Passar"))
        {
            isColliding = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
