using System.Collections.Generic;
using UnityEngine;

namespace TestInvaders.Level
{
    public class ProjectileBehaviour : PoolObject
    {
        public int TeamId { get; private set; }
        
        private float _speed;
        private Vector3 _direction;
        
        private Vector3 _velocity;

        private Renderer _renderer;
        private List<ProjectileBehaviour> _projectiles;
        
        public void SetupProjectile(int teamId, float speed, Vector3 direction, List<ProjectileBehaviour> projectiles)
        {
            TeamId = teamId;
            _speed = speed;
            _direction = direction;
            
            _velocity = direction.normalized * speed;

            _renderer = GetComponentInChildren<Renderer>();
            _projectiles = projectiles;
            
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        void Update()
        {
            transform.position += _velocity * Time.deltaTime;

            if (!_renderer.isVisible)
            {
                SelfDestroy();
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var projectile = other.GetComponent<ProjectileBehaviour>();
            if (projectile != null)
            {
                if (projectile.TeamId != TeamId)
                {
                    SelfDestroy();
                    return;
                }
            }
            
            var character = other.gameObject.GetComponent<CharacterBehaviour>();
            if (character != null)
            {
                if (character.TeamId != TeamId)
                {
                    SelfDestroy();
                }
            }
        }

        private void SelfDestroy()
        {
            _projectiles?.Remove(this);
            
            Destroy();
        }
    }
}

