using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamestateManager : MonoBehaviour
{
    public static GamestateManager Instance;

    private double timer;
    public TextMeshProUGUI timerText;

    public int totalItems = 8;
    public int remainingItems;
    public TextMeshProUGUI itemsCounterText;

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this; // Assign Instance at Awake to ensure it's available early
    }

    void Start()
    {
        remainingItems = totalItems;
        StartCoroutine(UpdateTimer());
    }

    IEnumerator UpdateTimer()
    {
        while (!isGameOver)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt((float)timer / 60F);
        int seconds = Mathf.FloorToInt((float)timer - minutes * 60);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = "Time: " + timerString + " / 2:30";
    }

    void Update()
    {
        UpdateItemsCounterUI(remainingItems);
        //and current scene is not deixalleria
        if ((timer >= 150 || remainingItems == 0) && SceneManager.GetActiveScene().name != "deixalleria")
        {
            LoadScene("deixalleria");
            remainingItems = totalItems;
        }

        else if ((timer >= 150 && remainingItems != 0) && SceneManager.GetActiveScene().name == "deixalleria")
        {
            //isGameOver = true;
            LoadScene("Lose");
        } else if (remainingItems == 0 && SceneManager.GetActiveScene().name == "deixalleria")
        { 
        
                //isGameOver = true;
            LoadScene("Win");
            
        }

        /*
        {
            LoadScene("deixalleria");
            remainingItems = totalItems;
        }
        */
    }

    public void UpdateItemsCounterUI(int remainingItems)
    {
        if (itemsCounterText != null)
        {
            itemsCounterText.text = "Remaining Items: " + remainingItems + " / " + totalItems;
        }
        else
        {
            Debug.LogWarning("Items counter text reference is not set in GamestateManager.");
        }
    }

    public void DecrementRemainingItems()
    {
        remainingItems--;
        UpdateItemsCounterUI(remainingItems);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
