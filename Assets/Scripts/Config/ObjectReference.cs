using System;
using UnityEngine.AddressableAssets;

namespace TestInvaders.Config
{
    [Serializable]
    public class ObjectReference
    {
        public ObjectType ObjectType;
        public AssetReference AssetReference;
    }
}