using System;
using Features.Spawner;
using Features.Spawner.Impl;
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

        public override void InstallBindings()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable), typeof(IBotSpawner)).To<BotSpawner>().FromInstance(_botSpawner).AsCached();
            Container.Bind<IBotSpawnFacade>().To<BotSpawnFacade>().AsCached();
            Container.Bind(typeof(IBotModel)).To<BotModel>().AsCached();
        }
    }
}