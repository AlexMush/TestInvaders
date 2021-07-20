using System;
using System.Collections.Generic;
using TestInvaders.Config;
using UnityEngine;

namespace TestInvaders.Level
{
    public partial class CharacterBehaviour : PoolObject
    {
        private Collider Collider { get; set; }
        private Renderer Renderer { get; set; }
        
        private ShootCharacterState ShootState { get; set; }
        private HitCharacterState HitState { get; set; }

        private List<ProjectileBehaviour> Projectiles { get; set; } = new List<ProjectileBehaviour>();
        private Func<ProjectileBehaviour> ProjectileCreator { get; set; }
        private Vector3 ShootDirection { get; set; }
        private float ProjectileSpeed { get; set; }
        private float ReloadDuration { get; set; }

        private float MovingSpeed { get; set; }
        private float InvincibleDuration { get; set; }
        private float BlinkingPeriod { get; set; }
        private int MaxLives { get; set; }

        private int _lives;
        private int Lives
        {
            get => _lives;
            set
            {
                if (_lives != value)
                {
                    _lives = value;
                    OnLivesChanged?.Invoke(_lives);
                }
            }
        }
        
        public int TeamId { get; private set; }
        public ObjectType ObjectType { get; set; }
        public bool IsAlive => Lives > 0;

        public event Action<CharacterBehaviour> OnDestroy;
        public event Action<int> OnLivesChanged; 
        
        public void SetupCharacter(CharacterConfig config, Func<ProjectileBehaviour> projectileCreator)
        {
            ProjectileCreator = projectileCreator;
            ShootDirection = config.ShootDirection;
            ProjectileSpeed = config.ProjectileSpeed;
            ReloadDuration = config.ReloadDuration;

            MovingSpeed = config.MovingSpeed;
            TeamId = config.TeamId;
            MaxLives = config.MaxLives;
            InvincibleDuration = config.InvincibleDuration;
            BlinkingPeriod = config.BlinkingPeriod;

            Collider = GetComponent<Collider>();
            Renderer = GetComponentInChildren<Renderer>();
            
            ResetCharacter();
        }

        public void ResetCharacter()
        {
            Enable(true);
            HitState = new FragileState(this);
            ShootState = new LoadedState(this);
            Lives = MaxLives;
            Renderer.enabled = true;

            foreach (var projectile in Projectiles)
            {
                projectile?.Destroy();
            }
            Projectiles.Clear();
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            
            HitState?.Update(dt);
            ShootState?.Update(dt);
        }

        private void HandleHit()
        {
            HitState.HandleHit();
        }

        public void Move(Vector3 moveDir)
        {
            var velocity = MovingSpeed * moveDir.normalized;
            transform.position += velocity * Time.deltaTime;
        }

        public void Shoot()
        {
            ShootState.Shoot();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                if (projectile.TeamId != TeamId)
                {
                    HandleHit();
                }
            }
        }

        public override void Destroy()
        {
            OnDestroy?.Invoke(this);
            
            Enable(false);
        }
    }
}
