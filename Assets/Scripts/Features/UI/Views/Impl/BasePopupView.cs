using System;
using UnityEngine;

namespace Features.UI.Views.Impl
{
    public abstract class BasePopupView : MonoBehaviour
    {
        public event Action ViewDestroyed = delegate { };

        public void CloseView()
        {
            ViewDestroyed.Invoke();
            Destroy(gameObject);
        }
    }
}