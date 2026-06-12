using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ClassicPlatformer
{
    public enum GameState
    {
        Playing,
        Paused,
        Won,
        Lost
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _coinsText;

        private int _coins;
        private GameState _state = GameState.Playing;

        public int Coins => _coins;
        public GameState State => _state;

        public event Action<GameState> StateChanged;
        public event Action<int> CoinsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SetState(GameState.Playing);
            UpdateCoinsUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePause();

            if ((_state == GameState.Won || _state == GameState.Lost) && Input.GetKeyDown(KeyCode.R))
                RestartLevel();
        }

        public void AddCoins(int amount)
        {
            _coins += amount;
            CoinsChanged?.Invoke(_coins);
            UpdateCoinsUI();
        }

        public void WinGame()
        {
            if (_state != GameState.Playing)
                return;

            SetState(GameState.Won);
            Time.timeScale = 0f;
        }

        public void LoseGame()
        {
            if (_state != GameState.Playing)
                return;

            SetState(GameState.Lost);
            Time.timeScale = 0f;
        }

        public void TogglePause()
        {
            if (_state == GameState.Playing)
            {
                SetState(GameState.Paused);
                Time.timeScale = 0f;
            }
            else if (_state == GameState.Paused)
            {
                SetState(GameState.Playing);
                Time.timeScale = 1f;
            }
        }

        public void RestartLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void SetState(GameState newState)
        {
            _state = newState;
            StateChanged?.Invoke(_state);
        }

        private void UpdateCoinsUI()
        {
            if (_coinsText != null)
                _coinsText.text = $"Coins: {_coins}";
        }
    }
}
