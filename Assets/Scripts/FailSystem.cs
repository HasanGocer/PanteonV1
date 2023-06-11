using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FailSystem : MonoSingleton<FailSystem>
{
    [SerializeField] GameObject _failPanel;
    [SerializeField] GameObject _gameboard;
    [SerializeField] Button _mainMenuButton, _restartButton;

    public void Start()
    {
        _mainMenuButton.onClick.AddListener(MainMenuButton);
        _restartButton.onClick.AddListener(RestartButton);
    }

    public void FailTime()
    {
        _gameboard.SetActive(false);
        _failPanel.SetActive(true);
        PlayerPrefs.DeleteAll();
        GameManager.Instance.gameStat = GameManager.GameStat.finish;
    }
    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        PlayerPrefs.DeleteAll();
        Load.Instance.isReturn = false;
        SceneManager.LoadScene(1);
    }
}
