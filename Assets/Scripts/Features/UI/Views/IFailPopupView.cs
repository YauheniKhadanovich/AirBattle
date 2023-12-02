using System;

namespace Features.UI.Views
{
    public interface IFailPopupView : IView
    {
        event Action RestartClicked;
    }
}