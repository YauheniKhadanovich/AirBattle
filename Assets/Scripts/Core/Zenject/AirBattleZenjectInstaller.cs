using System;
using Features.Aircraft.Controllers;
using Features.Aircraft.Controllers.Impl;
using Features.Aircraft.View;
using Features.Plane;
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
        public PlaneView _planeView;
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
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IAircraftController)).To<AircraftController>().AsCached();
            Container.Bind( typeof(IPlaneView)).FromInstance(_planeView).AsCached();
            Container.Bind<ViewFactory>().FromInstance(_viewFactory).AsCached();
            InstallViews();
            InstallPresenters();
            InstallFeatures();
            InstallModules();
        }

        private void InstallViews()
        {
            Container.Bind<IViewManager>().To<ViewManager>().AsSingle();
            Container.Bind<IMainGameView>().To<MainGameView>().FromInstance(_gameView).AsCached();
        }

        private void InstallPresenters()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IMainGamePresenter)).To<MainGamePresenter>().AsCached();
        }

        private void InstallFeatures()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameSpawner)).To<GameSpawner>().FromInstance(_gameSpawner).AsCached();
        }

        private void InstallModules()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameControllerFacade)).To<GameControllerFacade>().AsCached();
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IGameModel)).To<GameModel>().AsCached().WithArguments(_levelsRepository, _aircraftRepository);
        }
    }
}