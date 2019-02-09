using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private GameObject _inGameUIPanel;
    [SerializeField]
    private Text _gameOverPanelScoreText;
    [SerializeField]
    private Text _inGameScoreText;
    [SerializeField]
    private GameObject _joysticks;

    private void Start()
    {
        ToggleInGameUI(true);
        //only use touch joystick on android
#if UNITY_ANDROID
        _joysticks.SetActive(true);
#else
        joysticks.SetActive(false);
#endif
    }

    private void ToggleAllPanels(bool enable)
    {
        _pausePanel.SetActive(enable);
        _gameOverPanel.SetActive(enable);
        _inGameUIPanel.SetActive(enable);
    }
    public void TogglePausePanel(bool enable)
    {
        ToggleAllPanels(false);
        _pausePanel.SetActive(enable);
        
        _inGameUIPanel.SetActive(!enable);

    }

    public void ToggleGameOverPanel(bool enable)
    {
        //SetGameOverPanelScore(GameManager.Instance.tanksDestroyed);
        ToggleAllPanels(false);
        _gameOverPanel.SetActive(enable);
    }

    public void ToggleInGameUI(bool enable)
    {
        ToggleAllPanels(false);
        _inGameUIPanel.SetActive(enable);
    }

    public void UpdateScoreText(int score)
    {
        _gameOverPanelScoreText.text = "" + score;
        _inGameScoreText.text = "" + score;
    }

    public void ToggleJoystick(bool enable)
    {
        _joysticks.SetActive(true);
    }

    public void PauseButtonClick()
    {
        TogglePausePanel(true);
        GameManager.Instance.Gamestate = GameState.InMenu;
    }

    public void ResumeButtonClick()
    {
        GameManager.Instance.Gamestate = GameState.InGame;
        TogglePausePanel(false);
    }

    public void RestartButtonClick()
    {
        GameManager.Instance.Restart();
    }

    public void HomeButtonClick()
    {
        GameManager.Instance.BackToHomeScene();
    }
}
