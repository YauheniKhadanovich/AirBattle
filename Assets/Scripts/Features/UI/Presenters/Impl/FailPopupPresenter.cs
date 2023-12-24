using System;
using Features.UI.Views;
using Modules.GameController.Facade;

namespace Features.UI.Presenters.Impl
{
    public class FailPopupPresenter : IFailPopupPresenter
    {
        private readonly IGameControllerFacade _gameControllerFacade;
        private readonly IFailPopupView _view;
        
        public FailPopupPresenter(IGameControllerFacade gameControllerFacade, IFailPopupView view)
        {
            _gameControllerFacade = gameControllerFacade ?? throw new ArgumentNullException(nameof(gameControllerFacade));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }
        
        public void Initialize()
        {
            _view.RestartClicked += OnRestartClicked;
        }

        public void Dispose()
        {
            _view.RestartClicked -= OnRestartClicked;
        }

        private void OnRestartClicked()
        {
            _gameControllerFacade.ReportStartClicked(true);
        }
    }
}