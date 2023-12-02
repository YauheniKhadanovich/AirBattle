using System;
using Features.UI.ViewManagement;
using Features.UI.Views;
using Modules.GameController.Facade;
using Modules.GameController.Models.Impl;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class MainGamePresenter : IMainGamePresenter, IInitializable, IDisposable
    {
        [Inject] 
        private IViewManager _viewManager;
        [Inject] 
        private IMainGameView _mainGameView;
        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        public void Initialize()
        {
            _mainGameView.GoClicked += OnGoClicked;
            _gameControllerFacade.GameFailed += OnGameFailed;
            _gameControllerFacade.PointsUpdated += OnPointUpdated;
            _gameControllerFacade.LevelUpdated += OnLevelUpdated;
        }

        public void Dispose()
        {
            _mainGameView.GoClicked -= OnGoClicked;
            _gameControllerFacade.GameFailed -= OnGameFailed;
            _gameControllerFacade.PointsUpdated -= OnPointUpdated;
            _gameControllerFacade.LevelUpdated -= OnLevelUpdated;
        }

        private void OnGoClicked()
        {
            _gameControllerFacade.ReportStartClicked(false);
        }

        private void OnGameFailed()
        {
            _viewManager.OpenFailView();
        }

        private void OnPointUpdated(int pointsCount)
        {
            _mainGameView.SetPointsCount(pointsCount);
        }

        private void OnLevelUpdated(Level level)
        {
            _mainGameView.SetCurrentLevel(level);
        }
    }
}