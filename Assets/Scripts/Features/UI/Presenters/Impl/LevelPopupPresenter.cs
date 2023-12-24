using System;
using Features.UI.Views;
using Modules.GameController.Facade;

namespace Features.UI.Presenters.Impl
{
    public class LevelPopupPresenter : ILevelPopupPresenter
    {
        private readonly IGameControllerFacade _gameControllerFacade;
        private readonly ILevelPopupView _view;

        public LevelPopupPresenter(IGameControllerFacade gameControllerFacade, ILevelPopupView view)
        {
            _gameControllerFacade = gameControllerFacade ?? throw new ArgumentNullException(nameof(gameControllerFacade));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            _gameControllerFacade.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _gameControllerFacade.GameFailed -= OnGameFailed;
        }

        private void OnGameFailed()
        {
            _view.CloseView();
        }
    }
}