using UnityEngine;
using UnityEngine.UI;

public class TopPanelView : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    internal void SetScore(int score)
    {
        _scoreText.text = $"Score : {score}";
    }
}