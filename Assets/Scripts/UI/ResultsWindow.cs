using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestInvaders.UI
{
    public class ResultsWindowArgs
    {
        public int Score;
        public int Waves;
        public Action Back;
    }
    
    [Window("UI/ResultsWindow")]
    public class ResultsWindow : Window
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private TextMeshProUGUI _wavesText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private Action _back;
        
        protected override void OnShow(object args)
        {
            base.OnShow(args);

            var resultsWindowArgs = (ResultsWindowArgs) args;
            _back = resultsWindowArgs.Back;

            _backButton.onClick.AddListener(Back);

            _wavesText.text = resultsWindowArgs.Waves.ToString();
            _scoreText.text = resultsWindowArgs.Score.ToString();
        }

        protected override void OnHide()
        {
            base.OnHide();
            
            _backButton.onClick.RemoveAllListeners();
        }

        private void Back()
        {
            _back?.Invoke();
        }
    }
}