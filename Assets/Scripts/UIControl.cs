using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject inGameUIPanel;
    [SerializeField]
    private Text gameOverPanelScoreText;
    [SerializeField]
    private Text inGameScoreText;
    [SerializeField]
    private GameObject joysticks;

    private void Start()
    {
        ToggleInGameUI(true);
        //only use touch joystick on android
#if UNITY_ANDROID
        joysticks.SetActive(true);
#else
        joysticks.SetActive(false);
#endif
    }

    private void ToggleAllPanels(bool enable)
    {
        pausePanel.SetActive(enable);
        gameOverPanel.SetActive(enable);
        inGameUIPanel.SetActive(enable);
    }
    public void TogglePausePanel(bool enable)
    {
        ToggleAllPanels(false);
        pausePanel.SetActive(enable);
        
        inGameUIPanel.SetActive(!enable);

    }

    public void ToggleGameOverPanel(bool enable)
    {
        //SetGameOverPanelScore(GameManager.Instance.tanksDestroyed);
        ToggleAllPanels(false);
        gameOverPanel.SetActive(enable);
    }

    public void ToggleInGameUI(bool enable)
    {
        ToggleAllPanels(false);
        inGameUIPanel.SetActive(enable);
    }

    public void UpdateScoreText(int score)
    {
        gameOverPanelScoreText.text = "" + score;
        inGameScoreText.text = "" + score;
    }

    public void ToggleJoystick(bool enable)
    {
        joysticks.SetActive(true);
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
