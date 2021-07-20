using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TestInvaders.Level
{
    
    public class GameObjectPool<T> : IDisposable where T : PoolObject
    {
        public int MaxSize { get; private set; }
        public int ObjectsAmount { get; private set; }

        private readonly T _prefab;
        private readonly T[] _objects;

        public GameObjectPool(T prefab, int maxSize)
        {
            _prefab = prefab;
            MaxSize = maxSize;
            _objects = new T[MaxSize];
        }
        
        private T Create()
        {
            return Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        }

        private void Destroy(T o)
        {
            Object.Destroy(o);
        }

        private void Enable(T o)
        {
            o.gameObject.SetActive(true);
        }

        private void Disable(T o)
        {
            o.gameObject.SetActive(false);
        }
        
        public void Preload(int objectsCount)
        {
            if (objectsCount <= 0)
            {
                return;
            }
            var objects = new T[objectsCount];
            for (var i = 0; i < objectsCount; i++)
            {
                objects[i] = Acquire();
            }
            
            for (var i = 0; i < objectsCount; i++)
            {
                Release(objects[i]);
            }
        }

        public T Acquire()
        {
            T result;

            if (ObjectsAmount > 0)
            {
                --ObjectsAmount;
                result = _objects[ObjectsAmount];
                _objects[ObjectsAmount] = null;
                
                Enable(result);
            }
            else
            {
                result = Create();
            }

            return result;
        }
        
        public bool Release(T o)
        {
            Disable(o);

            if (ObjectsAmount >= MaxSize)
            {
                Destroy(o);
                return false;
            }

            _objects[ObjectsAmount] = o;
            ObjectsAmount++;
            return true;
        }
        
        public void Reset()
        {
            while (ObjectsAmount > 0)
            {
                ObjectsAmount--;
                var o = _objects[ObjectsAmount];
                _objects[ObjectsAmount] = null;
                
                Disable(o);
                Destroy(o);
            }
        }
        
        public void Dispose()
        {
            Reset();
        }
    }
}