using System;
using Features.Spawner;
using Features.Spawner.Impl;
using Features.UI.Presenters;
using Features.UI.Presenters.Impl;
using Features.UI.Views;
using Features.UI.Views.Impl;
using Modules.GameController.Facade;
using Modules.GameController.Facade.Impl;
using Modules.GameController.Models;
using Modules.GameController.Models.Impl;
using UnityEngine;
using Zenject;

namespace Core.Zenject
{
    public class TodoZenjectInstaller : MonoInstaller
    {
        [SerializeField] 
        private BotSpawner _botSpawner;
        [SerializeField] 
        private MainGameView _gameView;

        public override void InstallBindings()
        {
            InstallViews();
            InstallPresenters();
            InstallFeatures();
            InstallModules();
        }
        
        private void InstallViews()
        {
            Container.Bind<IMainGameView>().To<MainGameView>().FromInstance(_gameView).AsCached();
        }
        
        private void InstallPresenters()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IMainGamePresenter)).To<MainGamePresenter>().AsCached();
        }

        private void InstallFeatures()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IBotSpawner)).To<BotSpawner>().FromInstance(_botSpawner).AsCached();
        }
        
        private void InstallModules()
        {
            Container.Bind<IGameControllerFacade>().To<GameControllerFacade>().AsCached();
            Container.Bind(typeof(IGameModel)).To<GameModel>().AsCached();
        }
    }
}