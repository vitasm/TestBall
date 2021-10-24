using Scripts.Crystal;
using System;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Action<int, int, int> OnStartGame;
    public Action OnReStartGame;

    [SerializeField] private InfoPanelView _infoPanelView;
    [SerializeField] private MainPanelView _mainPanelView;
    [SerializeField] private TopPanelView _topPanelView;

    private int _score;

    private void Start()
    {
        _mainPanelView.ClickStartGame += ClickStartGame;
        _mainPanelView.ClickReStartGame += ClickReStartGame;
    }

    private void ClickStartGame(int speed, int width, int algorithm)
    {
        OnStartGame?.Invoke(speed, width, algorithm);
        _mainPanelView.gameObject.SetActive(false);
        _infoPanelView.gameObject.SetActive(true);
        _topPanelView.gameObject.SetActive(true);
        _score = 0;
        _topPanelView.SetScore(_score);
    }

    private void ClickReStartGame()
    {
        OnReStartGame?.Invoke();
        _mainPanelView.gameObject.SetActive(false);
        _infoPanelView.gameObject.SetActive(true);
        _topPanelView.gameObject.SetActive(true);
        _score = 0;
        _topPanelView.SetScore(_score);
    }

    public void ShowMenuStartGame()
    {
        _infoPanelView.gameObject.SetActive(false);
        _topPanelView.gameObject.SetActive(false);
        _mainPanelView.StartGame();
        _mainPanelView.gameObject.SetActive(true);
    }

    public void ShowMenuReStartGame()
    {
        StartCoroutine(ShowMenuReStartGameWaitSeconds(3));
    }

    public virtual IEnumerator ShowMenuReStartGameWaitSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        _infoPanelView.gameObject.SetActive(false);
        _topPanelView.gameObject.SetActive(false);
        _mainPanelView.ReStartGame(_score);
        _mainPanelView.gameObject.SetActive(true);
    }

    public void CollectedCrystal()
    {
        _score++;
        _topPanelView.SetScore(_score);
    }

    public void HideInfoLabel()
    {
        _infoPanelView.gameObject.SetActive(false);
    }
}