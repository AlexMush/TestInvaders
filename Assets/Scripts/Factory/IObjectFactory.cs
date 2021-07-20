using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestInvaders.Level
{
    public interface IObjectFactory : IDisposable
    {
        bool IsReady { get; }
        Task Setup(AssetReference reference);
        PoolObject CreateObject();
    }
}
