using System.Threading.Tasks;
using TestInvaders.UI;

namespace TestInvaders.Components
{
    public class LoadingComponent : IContextComponent
    {
        private IContext _context;
        private UIComponent _uiComponent;
        private LevelComponent _levelComponent;
        private StatisticsComponent _statisticsComponent;
        private ConfigComponent _configComponent;
        
        public void Initialize(IContext context)
        {
            _context = context;
            
            _configComponent = _context.GetContextComponent<ConfigComponent>();
            _uiComponent = _context.GetContextComponent<UIComponent>();
            _levelComponent = _context.GetContextComponent<LevelComponent>();
            _statisticsComponent = _context.GetContextComponent<StatisticsComponent>();
            
            _context.OnLoaded += LoadMenu;
            
            _uiComponent.Show<LoadingWindow>();
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }

        private void LoadMenu()
        {
            _uiComponent.Show<MenuWindow>(new MenuWindowArgs
            {
                StartGame = LoadLevel, 
                HighScores = ShowHighScores
            });
        }

        private void LoadLevel()
        {
            _uiComponent.Show<BattleWindow>(new BattleWindowArgs
            {
                Back = Back, 
                Lives = _configComponent.PlayerConfig.MaxLives
            });
            
            var battleWindow = _uiComponent.GetWindow<BattleWindow>();

            _statisticsComponent.ResetScore();
            _statisticsComponent.OnScoreChanged += battleWindow.OnScoreChanged;
            _levelComponent.OnWin += battleWindow.OnWin;
            _levelComponent.OnLose += ShowResults;
            _levelComponent.PlayerCharacter.OnLivesChanged += battleWindow.OnLivesChanged;
            _levelComponent.Start();
        }

        private void Back()
        {
            var battleWindow = _uiComponent.GetWindow<BattleWindow>();

            _statisticsComponent.OnScoreChanged -= battleWindow.OnScoreChanged;
            _levelComponent.OnWin -= battleWindow.OnWin;
            _levelComponent.OnLose -= ShowResults;
            _levelComponent.PlayerCharacter.OnLivesChanged -= battleWindow.OnLivesChanged;
            _levelComponent.Stop();
            
            LoadMenu();
        }

        private void ShowResults()
        {
            _uiComponent.Show<ResultsWindow>(new ResultsWindowArgs
            {
                Back = Back, 
                Score = _statisticsComponent.Score, 
                Waves = _levelComponent.WavesCleared
            });
        }

        private void ShowHighScores()
        {
            var scores = _statisticsComponent.GetScores();
            _uiComponent.Show<HighScoresWindow>(new HighScoresWindowArgs
            {
                Back = LoadMenu, 
                Scores = scores
            });
        }
    }
}