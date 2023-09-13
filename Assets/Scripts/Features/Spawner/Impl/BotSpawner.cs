using System;
using System.Collections;
using System.Collections.Generic;
using Features.Bots.Impl;
using Modules.GameController.Data;
using Modules.GameController.Facade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class BotSpawner : MonoBehaviour, IBotSpawner, IInitializable, IDisposable
    {
        private readonly Vector3 _groundPosition = Vector3.zero;
        private readonly Dictionary<BotType, Bot> _botPrefabsDict = new();

        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        [SerializeField] 
        private List<Transform> _spawnPositions;
        [SerializeField] 
        private List<BotPrefabsData> _botPrefabs;
        [SerializeField] 
        private Plane.PlaneView _planeView;

        public void Initialize()
        {
            _gameControllerFacade.SpawnBotRequested += OnSpawnBotRequested;
            _gameControllerFacade.GameStarted += OnGameStarted;
            _planeView.Failed += OnFailed;
        }

        private void Awake()
        {
            foreach (var botPrefabsData in _botPrefabs)
            {
                _botPrefabsDict.Add(botPrefabsData.BotType, botPrefabsData.BotPrefab);
            }
        }

        public void Dispose()
        {
            _gameControllerFacade.SpawnBotRequested -= OnSpawnBotRequested;
            _planeView.Failed -= OnFailed;
        }

        private void OnSpawnBotRequested(BotInfo botInfo)
        {
            StartCoroutine(SpawnCoroutine(botInfo));
        }
        
        private void OnGameStarted()
        {
            _planeView.InitPlane();
        }

        /*
         * TODO: refactoring. add something like queue, use jobs
         */
        private IEnumerator SpawnCoroutine(BotInfo botInfo)
        {
            yield return new WaitForSeconds(botInfo.SpawnDelay);
            
            var bot = Instantiate(_botPrefabsDict[botInfo.BotType], null);
            bot.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
            bot.transform.LookAt(_groundPosition);
            bot.BotSpawned += OnBotSpawned;
            _gameControllerFacade.DestroyBotsRequested += bot.FullDamage;
            bot.BotDestroyed += (botType, byPlayer) =>
            {
                _gameControllerFacade.OnBotDestroyed(botType, byPlayer);
                _gameControllerFacade.DestroyBotsRequested -= bot.FullDamage;
            };
            yield return null;
        }

        private void OnBotSpawned(BotType botType)
        {
            _gameControllerFacade.OnBotSpawned(botType);
        }

        private void OnFailed()
        {
            _gameControllerFacade.OnGameFailed();
        }
    }
}