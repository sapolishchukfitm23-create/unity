using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    // Win/Lose screen: appears when the game ends, shows the result
    // and collected coins, lets the player restart.
    public class EndGameScreen : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject _panel;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _resultText;

        private void Start()
        {
            GameManager.Instance.StateChanged += OnStateChanged;
            _panel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            if (state != GameState.Won && state != GameState.Lost)
            {
                _panel.SetActive(false);
                return;
            }

            _panel.SetActive(true);
            _titleText.text = state == GameState.Won ? "You Win!" : "Game Over";
            _resultText.text = $"Coins collected: {GameManager.Instance.Coins}";
        }

        // Hooked up to the Restart button (OnClick).
        public void OnRestartClicked()
        {
            GameManager.Instance.RestartLevel();
        }
    }
}
