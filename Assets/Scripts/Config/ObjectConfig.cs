using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TestInvaders.Config
{
    [CreateAssetMenu(fileName = "ObjectConfig", menuName = "ScriptableObjects/ObjectConfig")]
    public class ObjectConfig : ScriptableObject
    {
        public List<ObjectReference> ObjectReferences;

        public int MaxPoolSize = 50;
    }
}