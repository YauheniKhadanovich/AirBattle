using System;
using Features.Spawner;
using Features.Spawner.Impl;
using Features.UI.Presenters;
using Features.UI.Presenters.Impl;
using Features.UI.Views;
using Features.UI.Views.Impl;
using Modules.BotSpawn.Facade;
using Modules.BotSpawn.Facade.Impl;
using Modules.BotSpawn.Models;
using Modules.BotSpawn.Models.Impl;
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
            Container.Bind<IBotSpawnFacade>().To<BotSpawnFacade>().AsCached();
            Container.Bind(typeof(IBotModel)).To<BotModel>().AsCached();
        }
    }
}