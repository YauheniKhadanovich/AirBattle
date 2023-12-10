using System;

namespace Features.UI.Views
{
    public interface IMainGameView
    {
        event Action GoClicked;

        void ShowRestartButton();
        void SetPointsCount(int points);
        void SetLevelId(int levelId);
        void SetLevelProgress(int currentPoints, int targetPoints);
    }
}