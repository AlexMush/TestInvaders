using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestInvaders.Config;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestInvaders.Level
{
    public class FleetBehaviour : PoolObject
    {
        private CharacterBehaviour[,] _characters;
        private List<VanguardBehaviour> _vanguard = new List<VanguardBehaviour>();

        private int _rows;
        private int _columns;
        
        private float _halfWidth;

        private bool _isReady;
        private float _shootCooldown;
        private float _shootTimer;

        private Vector3 _spawnPos;
        private bool _isMovingRight = true;
        private float _xMin;
        private float _xMax;
        private float _movingSpeed;
        private float _step;

        public List<VanguardBehaviour> Vanguard => _vanguard;

        public async Task SetupFleet(GameConfig gameConfig, CharacterConfig characterConfig, 
            Func<ObjectType, CharacterBehaviour> characterCreator, Func<ProjectileBehaviour> projectileCreator)
        {
            _spawnPos = gameConfig.FleetSpawnPosition;
            _xMin = gameConfig.XPositionMin;
            _xMax = gameConfig.XPositionMax;
            _movingSpeed = characterConfig.MovingSpeed;
            _step = gameConfig.FleetStep;
            
            _rows = gameConfig.FleetRows;
            _columns = gameConfig.FleetColumns;
            
            _halfWidth = (_columns - 1) * gameConfig.FleetSpacing.x / 2;

            _shootCooldown = gameConfig.FleetShootCooldown;
            _shootTimer = _shootCooldown;
            
            _characters = new CharacterBehaviour[_rows, _columns];

            var pos = gameConfig.FleetStartPosition;
            for (var i = 0; i < _rows; i++)
            {
                pos.x = gameConfig.FleetStartPosition.x;
                
                for (var j = 0; j < _columns; j++)
                {
                    var character = characterCreator.Invoke(gameConfig.RowTypes[i]);
                    var characterTransform = character.transform;
                
                    characterTransform.SetParent(transform);
                    characterTransform.position = pos;
                
                    character.SetupCharacter(characterConfig, projectileCreator);

                    _characters[i, j] = character;
                    
                    pos.x += gameConfig.FleetSpacing.x;
                    
                    await Task.Yield();
                }
                
                pos.y += gameConfig.FleetSpacing.y;
            }

            _isReady = true;
        }

        public void ResetFleet()
        {
            transform.position = _spawnPos;
            _shootTimer = _shootCooldown;
            
            for (var i = 0; i < _rows; i++)
            {
                for (var j = 0; j < _columns; j++)
                {
                    _characters[i, j].ResetCharacter();
                }
            }
            
            _vanguard = new List<VanguardBehaviour>();
            for (var i = 0; i < _columns; i++)
            {
                _vanguard.Add(new VanguardBehaviour(_rows - 1, i, _characters, _vanguard));
            }
        }

        private void Update()
        {
            if (!_isReady || _vanguard.Count == 0)
            {
                return;
            }

            var dt = Time.deltaTime;
            Shoot(dt);
            Move(dt);
        }

        private void Shoot(float dt)
        {
            if (_shootTimer > 0)
            {
                _shootTimer -= dt;
            }
            else
            {
                _shootTimer = _shootCooldown;
                
                var rndId = Random.Range(0, _vanguard.Count);
                _vanguard[rndId].Shoot();
            }
        }

        private void Move(float dt)
        {
            if ( (transform.position.x + _halfWidth > _xMax && _isMovingRight) 
                || (transform.position.x - _halfWidth < _xMin && !_isMovingRight))
            {
                _isMovingRight = !_isMovingRight;
                transform.position += Vector3.down * _step;
            }
            
            var dir = _isMovingRight ? Vector3.right : Vector3.left;
            var velocity = _movingSpeed * dir;
            transform.position += velocity * dt;
        }
    }
}