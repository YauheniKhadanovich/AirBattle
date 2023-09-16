using System;
using System.Collections.Generic;
using System.Linq;
using Features.Bots.Impl;
using Modules.GameController.Facade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class BotSpawner : MonoBehaviour, IBotSpawner, IInitializable, IDisposable
    {
        private readonly Vector3 _groundPosition = Vector3.zero;

        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        [SerializeField] 
        private List<Transform> _spawnPositions;
        [SerializeField] 
        private Plane.PlaneView _planeView;

        public void Initialize()
        {
            _gameControllerFacade.GameStarted += OnGameStarted;
            _planeView.Failed += OnFailed;
        }
        
        public void Dispose()
        {
            _planeView.Failed -= OnFailed;
        }
        
        private void Update()
        {
            foreach (var botInfo in _gameControllerFacade.Bots.Select(pair => pair.Value))
            {
                botInfo.TimerTick(Time.deltaTime);

                if (!botInfo.IsNeedSpawn())
                {
                    continue;
                }

                Spawn(botInfo.BotId, botInfo.BotTo.BotPrefab);
                
                botInfo.Spawned();
            }
        }

        private void Spawn(string botId, Bot botPrefab)
        {
            var bot = Instantiate(botPrefab, null);
            bot.SetData(botId);
            bot.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
            bot.transform.LookAt(_groundPosition);
            _gameControllerFacade.DestroyBotsRequested += bot.FullDamage;
            bot.BotDestroyed += (id, byPlayer) =>
            {
                _gameControllerFacade.Bots[id].ReduceSpawnedBotsCount();
                _gameControllerFacade.OnBotDestroyed(byPlayer);
                _gameControllerFacade.DestroyBotsRequested -= bot.FullDamage;
            };
        }

        private void OnGameStarted()
        {
            _planeView.InitPlane();
        }
        
        private void OnFailed()
        {
            _gameControllerFacade.OnGameFailed();
        }
    }
}