using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] static int playerLives = 3;
    [SerializeField] float DeathDelay = 4f;
    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;
    [SerializeField] static int score = 0;

    

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int CoinValue)
    {
        score += CoinValue;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            
            TakeLife();
            
        }
        else
        {
            StartCoroutine(ResetGameSession());
            
        }
    }

    private void TakeLife()
    {
        playerLives--;
        StartCoroutine(DeathWait());
        livesText.text = playerLives.ToString();
        
    }

    IEnumerator DeathWait()
    {
        
        yield return new WaitForSecondsRealtime(DeathDelay);
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(DeathDelay);
        SceneManager.LoadScene(0);
        playerLives = 3;
        score = 0;
        Destroy(gameObject);

    }
}
