using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Inject] 
        private IGameControllerFacade _gameControllerFacade;
        
        [SerializeField] 
        private Transform[] _spawnPositions;
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
         * TODO: refactoring. add something like queue
         */
        private IEnumerator SpawnCoroutine(BotInfo botInfo)
        {
            yield return new WaitForSeconds(botInfo.SpawnDelay);
            var botPrefab = _botPrefabs.First(item => item.BotType == botInfo.BotType).BotPrefab;
            var pos = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            var bot = Instantiate(botPrefab, null);
            bot.transform.position = pos;
            bot.transform.LookAt(_groundPosition);
            bot.BotSpawned += OnBotSpawned;
            bot.BotDestroyed += OnBotDestroyed;
            yield return null;
        }

        private void OnBotDestroyed(BotType botType, bool byPlayer)
        {
            _gameControllerFacade.OnBotDestroyed(botType, byPlayer);
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