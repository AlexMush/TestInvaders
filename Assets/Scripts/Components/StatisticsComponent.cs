using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestInvaders.Config;
using TestInvaders.Level;
using TestInvaders.Statistics;

namespace TestInvaders.Components
{
    public class StatisticsComponent : IContextComponent
    {
        private PersistenceController _persistenceController = new PersistenceController();
        
        private List<ObjectScore> _scores;
        
        private ScoreData _scoreData = new ScoreData();

        public int Score => _scoreData.Score;
        
        public event Action<int> OnScoreChanged;

        public void Initialize(IContext context)
        {
            var configComponent = context.GetContextComponent<ConfigComponent>();
            _scores = configComponent.GameConfig.Scores;
            
            var objectFactoryComponent = context.GetContextComponent<ObjectFactoryComponent>();
            objectFactoryComponent.OnCharacterCreated += OnCharacterCreated;
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }

        public void ResetScore()
        {
            _scoreData = new ScoreData();
        }

        public List<ScoreData> GetScores()
        {
            return _persistenceController.LoadScore().Scores;
        }

        private void OnCharacterCreated(CharacterBehaviour character)
        {
            character.OnDestroy += OnCharacterDestroy;
        }

        private void OnCharacterDestroy(CharacterBehaviour character)
        {
            if (character.ObjectType == ObjectType.Main)
            {
                _scoreData.Date = DateTime.Now.ToFileTimeUtc();
                _persistenceController.SaveScore(_scoreData);
                return;
            }
            
            var objectScore = _scores.Find(c => c.ObjectType == character.ObjectType);
            if (objectScore != null)
            {
                _scoreData.Score += objectScore.Score;
                OnScoreChanged?.Invoke(_scoreData.Score);
            }
        }
    }
}