using System;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelView : MonoBehaviour
{
    public event Action<int, int, int> ClickStartGame;
    public event Action ClickReStartGame;

    [SerializeField] private Text _resultText;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _restartButton;

    [SerializeField] private Slider _speedBall;
    [SerializeField] private Slider _widthPath;
    [SerializeField] private Dropdown _algorithmCrystal;

    public void StartGame()
    {
        _resultText.gameObject.SetActive(false);
        _startButton.SetActive(true);
        _restartButton.SetActive(false);
    }

    public void ReStartGame(int score)
    {
        _resultText.gameObject.SetActive(true);
        _resultText.text = $"Game over\nScore: {score}";
        _startButton.SetActive(false);
        _restartButton.SetActive(true);
    }

    public void OnStartGame()
    {
        ClickStartGame?.Invoke((int)_speedBall.value, (int)_widthPath.value, (int)_algorithmCrystal.value);
    }

    public void OnReStartGame()
    {
        ClickReStartGame?.Invoke();
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}
