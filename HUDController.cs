using TMPro;
using UnityEngine;

namespace ClassicPlatformer
{
    // In-game HUD: health and coin counters. Subscribes to game events
    // instead of polling every frame.
    public class HUDController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Player _player;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _coinsText;

        private void Start()
        {
            _player.HealthChanged += OnHealthChanged;
            GameManager.Instance.CoinsChanged += OnCoinsChanged;

            OnHealthChanged(_player.CurrentHealth, _player.MaxHealth);
            OnCoinsChanged(GameManager.Instance.Coins);
        }

        private void OnDestroy()
        {
            if (_player != null)
                _player.HealthChanged -= OnHealthChanged;

            if (GameManager.Instance != null)
                GameManager.Instance.CoinsChanged -= OnCoinsChanged;
        }

        private void OnHealthChanged(int current, int max)
        {
            _healthText.text = $"HP: {current}/{max}";
        }

        private void OnCoinsChanged(int coins)
        {
            _coinsText.text = $"Coins: {coins}";
        }
    }
}
