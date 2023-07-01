using System;

namespace Features.UI.Views
{
    public interface IMainGameView
    {
        event Action GoClicked;
        event Action RestartClicked;

        void ShowRestartButton();
        void SetPointsCount(int points);
    }
}