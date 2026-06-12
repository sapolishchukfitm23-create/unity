using UnityEngine;

namespace ClassicPlatformer
{
    // Shows/hides the pause panel when the game state changes.
    // The actual pausing (Time.timeScale) lives in GameManager.
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;

        private void Start()
        {
            GameManager.Instance.StateChanged += OnStateChanged;
            _pausePanel.SetActive(false);
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameState state)
        {
            _pausePanel.SetActive(state == GameState.Paused);
        }

        // Hooked up to the Continue button (OnClick).
        public void OnContinueClicked()
        {
            GameManager.Instance.TogglePause();
        }

        // Hooked up to the Restart button (OnClick).
        public void OnRestartClicked()
        {
            GameManager.Instance.RestartLevel();
        }
    }
}
