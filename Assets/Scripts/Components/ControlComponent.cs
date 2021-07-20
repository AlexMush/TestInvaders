using System.Threading.Tasks;
using TestInvaders.Input;
using TestInvaders.Level;
using UnityEngine;

namespace TestInvaders.Components
{
    public class ControlComponent : IContextComponent
    {
        private IContext _context;
        private ConfigComponent _configComponent;

        private CharacterBehaviour _playerCharacter;

        private StandaloneInput _input = new StandaloneInput();

        private bool _isActive;
        
        public void Initialize(IContext context)
        {
            _context = context;
            _configComponent = _context.GetContextComponent<ConfigComponent>();

            _context.OnUpdate += OnUpdate;
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }

        public void SetPlayerCharacter(CharacterBehaviour character)
        {
            _playerCharacter = character;
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;
        }

        private void OnUpdate(float dt)
        {
            if (!_isActive || _playerCharacter == null)
            {
                return;
            }
            
            var inputInfo = _input.Update();
            if (inputInfo.IsShooting)
            {
                _playerCharacter.Shoot();
            }

            var isMoving = inputInfo.MoveDir != Vector2.zero;

            if (!isMoving)
            {
                return;
            }
            
            var playerPos = _playerCharacter.transform.position;
            var isBeforeLeftBound = playerPos.x > _configComponent.GameConfig.XPositionMin;
            var isBeforeRightBound = playerPos.x < _configComponent.GameConfig.XPositionMax;
            var isMovingRight = inputInfo.MoveDir.x > 0;
            
            if ((isBeforeLeftBound || isMovingRight) && (isBeforeRightBound || !isMovingRight))
            {
                _playerCharacter.Move(inputInfo.MoveDir);
            }
        }
    }
}