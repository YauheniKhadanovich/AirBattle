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
        private IMainGameView _view;
        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        public void Initialize()
        {
            _view.GoClicked += OnGoClicked;
            _gameControllerFacade.GameFailed += OnGameFailed;
            _gameControllerFacade.PointsUpdated += OnPointUpdated;
            _gameControllerFacade.LevelUpdated += OnLevelUpdated;
            _gameControllerFacade.LevelProgressUpdated += OnLevelProgressUpdated;
        }

        public void Dispose()
        {
            _view.GoClicked -= OnGoClicked;
            _gameControllerFacade.GameFailed -= OnGameFailed;
            _gameControllerFacade.PointsUpdated -= OnPointUpdated;
            _gameControllerFacade.LevelUpdated -= OnLevelUpdated;
            _gameControllerFacade.LevelProgressUpdated -= OnLevelProgressUpdated;
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
            _view.SetPointsCount(pointsCount);
        }

        private void OnLevelUpdated(int levelId)
        {
            _view.SetLevelId(levelId);
            _viewManager.OpenLevelView(levelId);
        }
        
        private void OnLevelProgressUpdated(int currentPoints, int targetPoints)
        {
            _view.SetLevelProgress(currentPoints, targetPoints);
        }
    }
}