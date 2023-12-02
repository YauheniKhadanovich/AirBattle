using System;

namespace Features.UI.Views
{
    public interface IView
    {
        event Action ViewDestroyed;

        void CloseView();
    }
}