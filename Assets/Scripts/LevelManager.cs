using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void PlayGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(NewGame());
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator NewGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOverScene()
    {
        SceneManager.LoadScene(2);
    }

    public void GameVictoryScene()
    {
        SceneManager.LoadScene(3);
    }
}
