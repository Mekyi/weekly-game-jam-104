using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject boxSpawner;
    [SerializeField] GameObject audioManager;
    [SerializeField] Animator[] beltAnimators;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] int startTime;
    [SerializeField] int gameTime;
    [SerializeField] int roundLength;
    [SerializeField] AudioClip clipRoundStart;
    [SerializeField] AudioClip[] roundFinishSound;
    [SerializeField] float speedChange = 1.5f;
    [SerializeField] float frequencyChange = 0.3f;
    [SerializeField] float[] roundSpeeds = new float[] { 3f, 4.5f, 6f, 7.5f, 9f };
    [SerializeField] float[] roundFrequencys = new float[] { 1.75f, 1.5f, 1f, 0.5f, 0.5f };

    public float boxSpeed = 5f;
    public float boxFrequency = 1f;

    AudioSource audioSource;
    bool gameStarted = false;
    bool gameRunning = false;
    bool startSoundPlayed = false;
    int roundTime;
    public int roundCount = 1;
    int score = 0;
    int health = 6;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        timeText = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();
        roundTime = roundLength;
        StartCoroutine(Timer());
        ChangeBeltSpeed(1);
    }


    // Update is called once per frame
    void Update()
    {
        UpdateCanvas();
        if (!gameStarted)
        {
            CheckGameStart();
        }
        if (gameStarted)
        {
            CheckRoundStart();
            if (roundCount == 6)
            {
                WinGame();
            }
        }
    }

    private void CheckRoundStart()
    {
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
        if (roundTime <= 0 && boxes.Length == 0)
        {
            StartCoroutine(RoundPause());
        }
        else if (roundTime <= 0)
        {
            gameRunning = false;
            boxSpawner.SetActive(false);
        }
    }

    IEnumerator RoundPause()
    {
        roundCount++;
        roundTime = roundLength;
        boxSpeed = roundSpeeds[roundCount-1];
        boxFrequency = roundFrequencys[roundCount-1];
        audioSource.clip = roundFinishSound[UnityEngine.Random.Range(0, roundFinishSound.Length)];
        audioSource.PlayDelayed(1);
        FindObjectOfType<AudioManager>().StopMusic();
        yield return new WaitForSeconds(3f);
        gameRunning = true;
        boxSpawner.SetActive(true);
        FindObjectOfType<AudioManager>().PlayMusic(roundCount - 1);
    }

    private void ChangeBeltSpeed(int speed)
    {
        foreach (Animator belt in beltAnimators)
        {
            belt.speed = speed;
        }
    }

    private void CheckGameStart()
    {
        if (!gameStarted && startTime <= 0)
        {
            gameStarted = true;
            gameRunning = true;
            boxSpeed = roundSpeeds[0];
            boxFrequency = roundFrequencys[0];
            boxSpawner.SetActive(true);
            audioManager.SetActive(true);
        }
        else if (!gameStarted && startTime == 3 && !startSoundPlayed)
        {
            startSoundPlayed = true;
            audioSource.PlayOneShot(clipRoundStart, 0.7f);
        }
    }

    IEnumerator Timer()
    {
        startTime -= 1;
        if (gameStarted && gameRunning)
        {
            gameTime -= 1;
            roundTime -= 1;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Timer());
    }

    private void WinGame()
    {
        FindObjectOfType<LevelManager>().GameVictoryScene();
    }

    private void UpdateCanvas()
    {
        scoreText.text = "Score: " + score.ToString();
        timeText.text = gameTime.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
    }

    public void TakeHealth()
    {
        health--;
        FindObjectOfType<HealthBar>().ChangeSprite(health);
        if (health < 1)
        {
            StartCoroutine(GameOver());
        }
    }

    public void GiveHealth()
    {
        health++;
        health = Mathf.Clamp(health, 0, 6);
        FindObjectOfType<HealthBar>().ChangeSprite(health);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(.5f);
        FindObjectOfType<LevelManager>().GameOverScene();
    }
}
