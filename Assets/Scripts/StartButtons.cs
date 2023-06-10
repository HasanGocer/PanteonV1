using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    [SerializeField] Button _startButton, _continueButton, _quitButton;

    void Start()
    {
        _startButton.onClick.AddListener(StartButton);
        _continueButton.onClick.AddListener(ContinueButton);
        _quitButton.onClick.AddListener(QuitButton);
    }

    private void StartButton()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
    }
    private void ContinueButton()
    {
        if (PlayerPrefs.HasKey("first"))
        {
            Load.Instance.isReturn = true;
            SceneManager.LoadScene(1);
        }
    }
    private void QuitButton()
    {
        Application.Quit();
    }
}
