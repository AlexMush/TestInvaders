using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestInvaders.UI
{
    public class BattleWindowArgs
    {
        public Action Back;
        public int Lives;
    }
    
    [Window("UI/BattleWindow")]
    public class BattleWindow : Window
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _currentWaveText;
        [SerializeField] private TextMeshProUGUI _currentScoreText;
        [SerializeField] private TextMeshProUGUI _livesLeftText;

        private Action _exit;

        private int _currentWave;
        private int _score;
        private int _lives;
        
        protected override void OnShow(object args)
        {
            base.OnShow(args);

            var battleArgs = (BattleWindowArgs) args;
            _exit = battleArgs.Back;
            
            _exitButton.onClick.AddListener(Exit);

            _currentWave = 1;
            _score = 0;
            _lives = battleArgs.Lives;

            _currentWaveText.text = _currentWave.ToString();
            _currentScoreText.text = _score.ToString();
            _livesLeftText.text = _lives.ToString();
        }
        
        protected override void OnHide()
        {
            base.OnHide();
            
            _exitButton.onClick.RemoveAllListeners();
            
            _currentWave = 0;
            _score = 0;
            _lives = 0;
            
            _currentWaveText.text = string.Empty;
            _currentScoreText.text = string.Empty;
            _livesLeftText.text = string.Empty;
        }

        private void Exit()
        {
            _exit?.Invoke();
        }

        public void OnWin()
        {
            _currentWave++;
            _currentWaveText.text = _currentWave.ToString();
        }

        public void OnLivesChanged(int lives)
        {
            _lives = lives;
            _livesLeftText.text = _lives.ToString();
        }

        public void OnScoreChanged(int score)
        {
            _score = score;
            _currentScoreText.text = _score.ToString();
        }
    }
}