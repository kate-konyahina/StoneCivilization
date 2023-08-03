using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _rulesPanel;

    public void Pause()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Rules()
    {
        _rulesPanel.SetActive(true);
        _pausePanel.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void BackToMenu()
    {
        _rulesPanel.SetActive(false);
        _pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
