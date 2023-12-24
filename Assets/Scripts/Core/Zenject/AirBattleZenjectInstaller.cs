using System;
using Features.Aircraft.Presenters;
using Features.Aircraft.Presenters.Impl;
using Features.Aircraft.View;
using Features.Aircraft.View.Impl;
using Features.Spawner;
using Features.Spawner.Impl;
using Features.UI.Presenters;
using Features.UI.Presenters.Impl;
using Features.UI.ViewManagement;
using Features.UI.ViewManagement.Impl;
using Features.UI.Views;
using Features.UI.Views.Impl;
using Modules.GameController.Facade;
using Modules.GameController.Facade.Impl;
using Modules.GameController.Models;
using Modules.GameController.Models.Impl;
using Modules.GameController.Repositories.Impl;
using UnityEngine;
using Zenject;

namespace Core.Zenject
{
    public class AirBattleZenjectInstaller : MonoInstaller
    {
        [SerializeField] 
        private ObjectPoolController _objectPoolController;
        [SerializeField] 
        private AircraftView _aircraftView;
        [SerializeField] 
        private ViewFactory _viewFactory;
        [SerializeField] 
        private GameSpawner _gameSpawner;
        [SerializeField] 
        private MainGameView _gameView;
        [SerializeField] 
        private LevelsRepository _levelsRepository;
        [SerializeField] 
        private AircraftRepository _aircraftRepository;
        
        public override void InstallBindings()
        {
            InstallViews();
            InstallPresenters();
            InstallFeatures();
            InstallModules();
        }

        private void InstallFeatures()
        {
            Container.Bind<IObjectPoolController>().FromInstance(_objectPoolController).AsCached();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameSpawner)).To<GameSpawner>().FromInstance(_gameSpawner).AsCached();
        }

        private void InstallViews()
        {
            Container.Bind( typeof(IAircraftView)).FromInstance(_aircraftView).AsCached();
            Container.Bind<ViewFactory>().FromInstance(_viewFactory).AsCached();
            Container.Bind<IViewManager>().To<ViewManager>().AsSingle();
            Container.Bind<IMainGameView>().To<MainGameView>().FromInstance(_gameView).AsCached();
        }

        private void InstallPresenters()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable),  typeof(ITickable), typeof(IAircraftPresenter)).To<AircraftPresenter>().AsCached();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IMainGamePresenter)).To<MainGamePresenter>().AsCached();
        }

        private void InstallModules()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameControllerFacade)).To<GameControllerFacade>().AsCached();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameModel)).To<GameModel>().AsCached().WithArguments(_levelsRepository, _aircraftRepository);
        }
    }
}