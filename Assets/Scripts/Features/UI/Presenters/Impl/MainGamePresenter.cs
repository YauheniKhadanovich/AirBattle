using System;
using Features.UI.ViewManagement;
using Features.UI.Views;
using Modules.GameController.Facade;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class MainGamePresenter : IMainGamePresenter, IInitializable, IDisposable
    {
        private IViewManager _viewManager;
        private IMainGameView _view;
        private IGameControllerFacade _gameControllerFacade;
        
        public MainGamePresenter(IGameControllerFacade gameControllerFacade, IMainGameView view, IViewManager viewManager)
        {
            _gameControllerFacade = gameControllerFacade ?? throw new ArgumentNullException(nameof(gameControllerFacade));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
        }

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