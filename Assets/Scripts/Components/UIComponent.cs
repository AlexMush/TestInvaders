using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TestInvaders.UI
{
    public class UIComponent : IContextComponent
    {
        private IContext _context;
        
        private Transform _canvasTransform;
        
        private readonly Dictionary<Type, Window> _windows = new Dictionary<Type, Window>();
        
        private Window _curActiveWindow;
        
        public void Initialize(IContext context)
        {
            _context = context;
            
            CreateUiEventSystem();
            _canvasTransform = CreateCanvas().transform;
        }

        public Task Load()
        {
            return Task.CompletedTask;
        }
        
        private void CreateUiEventSystem()
        {
            var objectEvent = new GameObject("UIEvent");
            objectEvent.AddComponent<EventSystem>();
            objectEvent.AddComponent<StandaloneInputModule>();
        }

        private GameObject CreateCanvas()
        {
            var objectCanvas = new GameObject("UICanvas");
            
            var canvas = objectCanvas.AddComponent<Canvas>();
            var scaler = objectCanvas.AddComponent<CanvasScaler>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 0;

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1.0f;
            scaler.referenceResolution = new Vector2(1920, 1080);

            objectCanvas.AddComponent<GraphicRaycaster>();
            
            return objectCanvas;
        }
        
        public T GetWindow<T>() where T : Window
        {
            var type = typeof(T);
            if (_windows.ContainsKey(type))
            {
                return _windows[type] as T;
            }

            Debug.LogError("ui component :: window not spawned: " + type);
            return null;
        }
        
        public void Show<T>(object args = null) where T : Window
        {
            var canvasTransform = _canvasTransform;

            var itemToShow = CacheWindow<T>(canvasTransform);
            if (itemToShow == null)
            {
                Debug.LogError($"prefab for ui with type {typeof(T).Name} not found");
                return;
            }

            ShowWindow(itemToShow, args);
        }

        private void ShowWindow(Window window, object args)
        {
            _curActiveWindow?.HideWindow();
            _curActiveWindow = window;
            _curActiveWindow.ShowWindow(_context, args);
        }
        
        private T CacheWindow<T>(Transform canvasTransform) where T : Window
        {
            var type = typeof(T);
            T result = null;

            if (!_windows.ContainsKey(type))
            {
                var windowAttribute = GetPrefabName<T>();

                result = InstantiateWindow<T>(windowAttribute.PrefabPath, canvasTransform);
                var isPrefabExists = result != null;
                if (isPrefabExists)
                {
                    _windows.Add(type, result);
                }
                else
                {
                    Debug.LogError("ui component :: prefab does not exists " + windowAttribute.PrefabPath);
                }
            }
            else
            {
                result = _windows[type] as T;
            }

            return result;
        }
        
        private T InstantiateWindow<T>(string prefabName, Transform canvasTransform) where T : Window
        {
            var prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogError("ui component :: cannot load window prefab with name = " + prefabName);
                return null;
            }

            var newWinInstance = Object.Instantiate(prefab, canvasTransform);
            var newWinComponent = newWinInstance.GetComponent<T>();
            return newWinComponent;
        }
        
        private WindowAttribute GetPrefabName<T>() where T : Window
        {
            if (Attribute.IsDefined(typeof(T), typeof(WindowAttribute)))
            {
                return Attribute.GetCustomAttribute(typeof(T), typeof(WindowAttribute)) as WindowAttribute;
            }

            Debug.LogError($"ui component :: window {typeof(T)} has no window attribute");
            return null;
        }
    }
}