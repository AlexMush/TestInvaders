using System;
using UnityEngine;
using UnityEngine.UI;

namespace TestInvaders.UI
{
    public class MenuWindowArgs
    {
        public Action StartGame;
        public Action HighScores;
    }
    
    [Window("UI/MenuWindow")]
    public class MenuWindow : Window
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _highScoresButton;

        private Action _startGame;
        private Action _highScores;

        protected override void OnShow(object args)
        {
            base.OnShow(args);

            var menuArgs = (MenuWindowArgs) args;
            _startGame = menuArgs.StartGame;
            _highScores = menuArgs.HighScores;

            _startGameButton.onClick.AddListener(StartGame);
            _highScoresButton.onClick.AddListener(HighScores);
        }

        protected override void OnHide()
        {
            base.OnHide();
            
            _startGameButton.onClick.RemoveAllListeners();
            _startGameButton.onClick.RemoveAllListeners();
        }

        private void StartGame()
        {
            _startGame?.Invoke();
        }
        
        private void HighScores()
        {
            _highScores?.Invoke();
        }
    }
}