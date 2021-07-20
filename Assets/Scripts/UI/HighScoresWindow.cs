using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TestInvaders.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace TestInvaders.UI
{
    public class HighScoresWindowArgs
    {
        public List<ScoreData> Scores;
        public Action Back;
    }
    
    [Window("UI/HighScoresWindow")]
    public class HighScoresWindow : Window
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private List<ScoreItem> _scoreItems;
        
        private Action _back;
        
        protected override void OnShow(object args)
        {
            base.OnShow(args);

            var highScoresWindowArgs = (HighScoresWindowArgs) args;
            _back = highScoresWindowArgs.Back;

            _backButton.onClick.AddListener(Back);

            var scores = highScoresWindowArgs.Scores;
            var sortedScores = scores.OrderByDescending(sd => sd.Score).ToList();

            for (var i = 0; i < _scoreItems.Count; i++)
            {
                var scoreItem = _scoreItems[i];
                
                if (i > sortedScores.Count - 1)
                {
                    scoreItem.gameObject.SetActive(false);
                    continue;
                }

                var scoreData = sortedScores[i];
                
                scoreItem.gameObject.SetActive(true);
                scoreItem.DateText.text = DateTime.FromFileTimeUtc(scoreData.Date).ToString(CultureInfo.InvariantCulture);
                scoreItem.ScoreText.text = scoreData.Score.ToString();
            }
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
        
#if UNITY_EDITOR
        public void SetupScoreItems()
        {
            var scoreItems = GetComponentsInChildren<ScoreItem>();
            _scoreItems = scoreItems.ToList();
        }
#endif
    }
}