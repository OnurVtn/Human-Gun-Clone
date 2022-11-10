using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

    [SerializeField] private bool isGameStart, isGameFinish;

    [SerializeField] private GameObject beforeGame, failPanel, completePanel;

    [SerializeField] private static int dollarAmount = 0;
    [SerializeField] private TextMeshProUGUI dollarText;

    [SerializeField] private static int levelNumber = 1;
    [SerializeField] private TextMeshProUGUI levelText;

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

    void Start()
    {
        dollarText.text = "$" + dollarAmount;
        levelText.text = "Level " + levelNumber;
    }

    public bool IsGameStart()
    {
        return isGameStart;
    }

    public bool IsGameFinish()
    {
        return isGameFinish;
    }

    public void OnGameStarted()
    {
        isGameStart = true;
        beforeGame.SetActive(false);
    }

    public void OnGameFailed()
    {
        isGameFinish = true;
        failPanel.SetActive(true);
    }

    public void OnGameCompleted()
    {
        isGameFinish = true;
        completePanel.SetActive(true);
    }

    public void IncreaseDollar()
    {
        dollarAmount ++;
        dollarText.text = "$" + dollarAmount;
    }

    public void RetryButton()
    {
        LevelManager.Instance.RestartLevel(levelNumber - 1);

        if (levelNumber == 1)
        {
            dollarAmount = 0;
        }      
    }

    public void NextButton()
    {
        LevelManager.Instance.LoadNextLevel(levelNumber);

        IncreaseLevel();
    }

    private void IncreaseLevel()
    {
        levelNumber ++;
        levelText.text = "Level " + levelNumber;
    }
}
