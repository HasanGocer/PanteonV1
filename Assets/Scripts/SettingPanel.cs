using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] GameObject _settingPanel;
    [SerializeField] GameObject _gameBoard;
    [SerializeField] Button _settingButton, _settingBackButton, _mainMenuButton, _restartButton, _quitButton;

    private void Start()
    {
        _settingButton.onClick.AddListener(SettingButton);
        _settingBackButton.onClick.AddListener(SettingButton);
        _mainMenuButton.onClick.AddListener(FailSystem.Instance.MainMenuButton);
        _restartButton.onClick.AddListener(FailSystem.Instance.RestartButton);
        _quitButton.onClick.AddListener(QuitButton);
    }

    private void SettingButton()
    {
        if (_settingPanel.activeInHierarchy)
        {
            Time.timeScale = 1;
            SoundSystem.Instance.EffectMusicPlacement();
            _settingPanel.SetActive(false);
            _gameBoard.SetActive(true);
        }
        else
        {
            SoundSystem.Instance.SetEffectMusicBar(SoundSystem.Instance.GetEffect());
            Time.timeScale = 0;
            _settingPanel.SetActive(true);
            _gameBoard.SetActive(false);
        }
    }
    private void QuitButton()
    {
        Application.Quit();
    }
}
