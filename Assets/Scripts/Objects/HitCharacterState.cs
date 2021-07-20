namespace TestInvaders.Level
{
    public partial class CharacterBehaviour : PoolObject
    {
        private abstract class HitCharacterState
        {
            protected CharacterBehaviour _owner;

            public HitCharacterState(CharacterBehaviour owner)
            {
                _owner = owner;
            }

            public virtual void OnExit()
            {
                
            }

            public virtual void Update(float dt)
            {
                
            }

            public virtual void HandleHit()
            {
                
            }
        }

        private class FragileState : HitCharacterState
        {
            public FragileState(CharacterBehaviour owner) : base(owner)
            {
            }

            public override void HandleHit()
            {
                _owner.Lives--;
                if (_owner.Lives > 0)
                {
                    _owner.HitState = new InvincibleState(_owner);
                }
                else
                {
                    _owner.HitState = null;
                    _owner.Destroy();
                }
                _owner = null;
            }
        }
        
        private class InvincibleState : HitCharacterState
        {
            private float _invincibleDuration;
            private float _blinkingPeriod;
            
            public InvincibleState(CharacterBehaviour owner) : base(owner)
            {
                _invincibleDuration = owner.InvincibleDuration;
                _blinkingPeriod = owner.BlinkingPeriod;
            }

            public override void OnExit()
            {
                _owner.Renderer.enabled = true;
            }

            public override void Update(float dt)
            {
                if (_blinkingPeriod > 0)
                {
                    _blinkingPeriod -= dt;
                }
                else
                {
                    _blinkingPeriod = _owner.BlinkingPeriod;
                    _owner.Renderer.enabled = !_owner.Renderer.enabled;
                }

                if (_invincibleDuration > 0)
                {
                    _invincibleDuration -= dt;
                }
                else
                {
                    OnExit();
                    _owner.HitState = new FragileState(_owner);
                    _owner = null;
                }
            }
        }
    }
    
}