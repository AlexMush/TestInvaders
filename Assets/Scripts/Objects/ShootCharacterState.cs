namespace TestInvaders.Level
{
    public partial class CharacterBehaviour
    {
        private abstract class ShootCharacterState
        {
            protected CharacterBehaviour _owner;

            public ShootCharacterState(CharacterBehaviour owner)
            {
                _owner = owner;
            }

            public virtual void Shoot()
            {
                
            }

            public virtual void Update(float dt)
            {
                
            }
        }

        private class LoadedState : ShootCharacterState
        {
            public LoadedState(CharacterBehaviour owner) : base(owner)
            {
            }

            public override void Shoot()
            {
                var projectile = _owner.ProjectileCreator?.Invoke();
                projectile.transform.position = _owner.transform.position;
                projectile.SetupProjectile(_owner.TeamId, _owner.ProjectileSpeed, _owner.ShootDirection, _owner.Projectiles);
                _owner.Projectiles.Add(projectile);
                
                _owner.ShootState = new ReloadingState(_owner);
                _owner = null;
            }
        }
        
        private class ReloadingState : ShootCharacterState
        {
            private float _reloadDuration;
            
            public ReloadingState(CharacterBehaviour owner) : base(owner)
            {
                _reloadDuration = owner.ReloadDuration;
            }

            public override void Update(float dt)
            {
                if (_reloadDuration > 0)
                {
                    _reloadDuration -= dt;
                }
                else
                {
                    _owner.ShootState = new LoadedState(_owner);
                    _owner = null;
                }
            }
        }
    }
    
}