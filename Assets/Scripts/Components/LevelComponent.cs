using System;
using System.Threading.Tasks;
using TestInvaders.Config;
using TestInvaders.Level;

namespace TestInvaders.Components
{
    public class LevelComponent : IContextComponent
    {
        private IContext _context;

        private GameConfig _gameConfig;
        private CharacterConfig _npcConfig;
        private CharacterConfig _playerConfig;
        
        private ObjectFactoryComponent _objectFactoryComponent;
        private ControlComponent _controlComponent;

        private FleetBehaviour _fleet;
        
        private CharacterBehaviour _playerCharacter;
        private int _wavesCleared;

        public event Action OnWin;
        public event Action OnLose;

        public CharacterBehaviour PlayerCharacter => _playerCharacter;
        public int WavesCleared => _wavesCleared;

        public void Initialize(IContext context)
        {
            _context = context;
            
            var configComponent = _context.GetContextComponent<ConfigComponent>();
            _gameConfig = configComponent.GameConfig;
            _npcConfig = configComponent.NpcConfig;
            _playerConfig = configComponent.PlayerConfig;

            _objectFactoryComponent = _context.GetContextComponent<ObjectFactoryComponent>();
            _controlComponent = _context.GetContextComponent<ControlComponent>();
        }

        public async Task Load()
        {
            _fleet = _objectFactoryComponent.CreateObject<FleetBehaviour>(ObjectType.Fleet);
            _fleet.transform.position = _gameConfig.FleetSpawnPosition;
            await _fleet.SetupFleet(_gameConfig, _npcConfig, 
                _objectFactoryComponent.CreateCharacterBehaviour, _objectFactoryComponent.CreateProjectileBehaviour);

            _playerCharacter = _objectFactoryComponent.CreateCharacterBehaviour(ObjectType.Main);
            _playerCharacter.transform.position = _gameConfig.PlayerSpawnPosition;
            _playerCharacter.SetupCharacter(_playerConfig, _objectFactoryComponent.CreateProjectileBehaviour);
            
            ResetLevel();
            
            _fleet.Enable(false);
            _playerCharacter.Enable(false);

            _controlComponent.SetPlayerCharacter(_playerCharacter);
        }

        public void Start()
        {
            _fleet.Enable(true);
            _playerCharacter.Enable(true);
            
            _controlComponent.Activate(true);

            _context.OnUpdate += OnUpdate;
        }

        public void Stop()
        {
            _controlComponent.Activate(false);
            
            ResetLevel();
         
            _wavesCleared = 0;
            
            _fleet.Enable(false);
            _playerCharacter.Enable(false);
            
            _context.OnUpdate -= OnUpdate;
        }

        private void ResetLevel()
        {
            _fleet.ResetFleet();
            _playerCharacter.ResetCharacter();

            _playerCharacter.transform.position = _gameConfig.PlayerSpawnPosition;
        }

        private void NextWave()
        {
            _fleet.ResetFleet();
        }

        private void OnUpdate(float dt)
        {
            var vanguard = _fleet.Vanguard;
            if (vanguard.Count == 0)
            {
                Win();
            }
            
            foreach (var character in vanguard)
            {
                if (character.Position.y < _gameConfig.FleetYPositionMin)
                {
                    Lose();
                }
            }
            
            if (!_playerCharacter.IsAlive)
            {
                Lose();
            }
        }

        private void Win()
        {
            OnWin?.Invoke();
            NextWave();
            _wavesCleared++;
        }

        private void Lose()
        {
            OnLose?.Invoke();
            Stop();
        }
    }
}