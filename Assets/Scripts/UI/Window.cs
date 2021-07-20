using UnityEngine;

namespace TestInvaders.UI
{
    public class Window : MonoBehaviour
    {
        public void ShowWindow(IContext context, object args = null)
        {
            gameObject.SetActive(true);
            OnShow(args);
        }

        public void HideWindow()
        {
            OnHide();
            gameObject.SetActive(false);
        }

        protected virtual void OnShow(object args)
        {
            
        }

        protected virtual void OnHide()
        {
            
        }
    }
}