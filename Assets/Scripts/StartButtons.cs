using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
    [SerializeField] Button _startButton, _continueButton, _quitButton, _infoButton, _infoBackButton;
    [SerializeField] GameObject _infoPanel;

    void Start()
    {
        _startButton.onClick.AddListener(StartButton);
        _continueButton.onClick.AddListener(ContinueButton);
        _quitButton.onClick.AddListener(QuitButton);
        _infoButton.onClick.AddListener(InfoButton);
        _infoBackButton.onClick.AddListener(InfoBackButton);
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
            GridSystem.MainGrid tempGrid = GridPlacementRead();

            if (!tempGrid.isFinish)
            {
                Load.Instance.isReturn = true;
                SceneManager.LoadScene(1);
            }
        }
    }
    private void InfoButton()
    {
        _infoPanel.SetActive(true);
    }
    private void InfoBackButton()
    {
        _infoPanel.SetActive(false);
    }
    private void QuitButton()
    {
        Application.Quit();
    }
    private GridSystem.MainGrid GridPlacementRead()
    {
        string jsonRead = System.IO.File.ReadAllText(Application.persistentDataPath + "/GridData.json");
        GridSystem.MainGrid grid = new GridSystem.MainGrid();
        grid = JsonUtility.FromJson<GridSystem.MainGrid>(jsonRead);
        return grid;
    }
}
