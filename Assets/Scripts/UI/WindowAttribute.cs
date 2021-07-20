using System;

namespace TestInvaders.UI
{
    public class WindowAttribute : Attribute
    {
        public WindowAttribute ( string prefabPath)
        {
            PrefabPath = prefabPath;
        }

        public string PrefabPath { get; private set; }
    }
}