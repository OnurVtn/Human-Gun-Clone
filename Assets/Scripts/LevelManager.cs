using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    public static LevelManager Instance => instance ?? (instance = FindObjectOfType<LevelManager>());

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartLevel(int currentLevel)
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadNextLevel(int nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }
}
