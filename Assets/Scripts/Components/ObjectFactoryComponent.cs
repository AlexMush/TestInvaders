using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestInvaders.Level;
using UnityEngine;

namespace TestInvaders.Components
{
    public class ObjectFactoryComponent : IContextComponent
    {
        private IContext _context;
        private ConfigComponent _configComponent;
        
        private readonly Dictionary<ObjectType, IObjectFactory> _factories = new Dictionary<ObjectType, IObjectFactory>();

        public event Action<CharacterBehaviour> OnCharacterCreated; 
        
        public bool IsReady
        {
            get
            {
                foreach (var pair in _factories)
                {
                    if (!pair.Value.IsReady)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        
        public void Initialize(IContext context)
        {
            _context = context;
            _configComponent = _context.GetContextComponent<ConfigComponent>();
        }

        public async Task Load()
        {
            var objConfig = _configComponent.ObjectConfig;
            var maxPoolSize = objConfig.MaxPoolSize;
            
            var objRefs = objConfig.ObjectReferences;
            foreach (var objRef in objRefs)
            {
                var objFactory = new PoolObjectFactory(maxPoolSize);
                await objFactory.Setup(objRef.AssetReference);
                
                _factories[objRef.ObjectType] = objFactory;
            }
        }
        
        public T CreateObject<T>(ObjectType objectType, Action<T> objectInitialization = null) where T : PoolObject
        {
            T result = null;
            if (!_factories.TryGetValue(objectType, out var factory))
            {
                Debug.LogError($"object factory component :: cannot find factory for object type = {objectType}");
            }
            else
            {
                try
                {
                    result = (T)factory.CreateObject();
                    objectInitialization?.Invoke(result);
                }
                catch (Exception e)
                {
                    Debug.LogError($"object factory component :: exception during creating object type {objectType}\n{e}");
                }
                
            }
            return result;
        }

        public CharacterBehaviour CreateCharacterBehaviour(ObjectType objectType)
        {
            var character = CreateObject<CharacterBehaviour>(objectType);
            character.ObjectType = objectType;
            
            OnCharacterCreated?.Invoke(character);
            
            return character;
        }
        
        public ProjectileBehaviour CreateProjectileBehaviour()
        {
            return CreateObject<ProjectileBehaviour>(ObjectType.Projectile);
        }
    }
}