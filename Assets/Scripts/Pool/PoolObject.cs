using UnityEngine;

namespace TestInvaders.Level
{
    public class PoolObject : MonoBehaviour
    {
        private GameObjectPool<PoolObject> _origin;
        
        public void Setup(GameObjectPool<PoolObject> origin)
        {
            _origin = origin;
        }

        public void Enable(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public virtual void Destroy()
        {
            _origin.Release(this);
        }
    }
}