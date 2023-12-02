using Features.UI.Views;
using Modules.GameController.Facade;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class FailPopupPresenter : IFailPopupPresenter
    {
        [Inject]
        private readonly IGameControllerFacade _gameControllerFacade;
        [Inject]
        private readonly IFailPopupView _view;
        
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