using Features.UI.Views;
using Modules.GameController.Facade;
using Zenject;

namespace Features.UI.Presenters.Impl
{
    public class LevelPopupPresenter : ILevelPopupPresenter
    {
        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        [Inject]
        private readonly ILevelPopupView _view;
        
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