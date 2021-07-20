using System;

namespace TestInvaders
{
    public interface IContext
    {
        event Action OnLoaded;
        event Action<float> OnUpdate;
        T GetContextComponent<T>() where T : class, IContextComponent;
    }
}