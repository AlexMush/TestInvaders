using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestInvaders.Level
{
    public class PoolObjectFactory : IObjectFactory
    {
        private GameObjectPool<PoolObject> _pool;
        private int _maxPoolSize;
        
        public bool IsReady { get; private set; }

        public PoolObjectFactory(int maxPoolSize)
        {
            _maxPoolSize = maxPoolSize;
        }
        
        public async Task Setup(AssetReference reference)
        {
            var getDownloadSize = Addressables.GetDownloadSizeAsync(reference);
            while (!getDownloadSize.IsDone)
            {
                await Task.Yield();
            }
            
            if (getDownloadSize.Result > 0)
            {
                var downloadDependencies = Addressables.DownloadDependenciesAsync(reference);
                while (!downloadDependencies.IsDone)
                {
                    await Task.Yield();
                }
            }
            
            var loadAsset = Addressables.LoadAssetAsync<GameObject>(reference);
            while (!loadAsset.IsDone)
            {
                await Task.Yield();
            }
            
            _pool = new GameObjectPool<PoolObject>(loadAsset.Result.GetComponent<PoolObject>(), _maxPoolSize);
            IsReady = true;
        }

        public PoolObject CreateObject()
        {
            var result = _pool.Acquire();
            result.Setup(_pool);
            return result;
        }

        public void Dispose()
        {
            _pool.Dispose();
        }
    }
}