using System;
using System.Collections.Generic;
using TestInvaders.Components;
using TestInvaders.UI;
using UnityEngine;

namespace TestInvaders
{
    public class GameContext : MonoBehaviour, IContext
    {
        public event Action OnLoaded;
        public event Action<float> OnUpdate;
        
        private List<IContextComponent> _components = new List<IContextComponent>();
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private async void Start()
        {
            _components = new List<IContextComponent>
            {
                new UIComponent(),
                new LoadingComponent(),
                new ConfigComponent(),
                new StatisticsComponent(),
                new ObjectFactoryComponent(),
                new LevelComponent(),
                new ControlComponent(),
            };
            
            foreach (var component in _components)
            {
                component.Initialize(this);
                await component.Load();
            }
            
            OnLoaded?.Invoke();
            OnLoaded = null;
        }

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime);
        }

        public T GetContextComponent<T>() where T : class, IContextComponent
        {
            T result = null;
            if (_components != null)
            {
                for (int i = 0, max = _components.Count; i < max; ++i)
                {
                    result = _components[i] as T;
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            return result;
        }
    }
}