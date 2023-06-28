using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Plane.Components;
using Modules.BotSpawn.Data;
using Modules.BotSpawn.Facade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class BotSpawner : MonoBehaviour, IBotSpawner, IInitializable, IDisposable
    {
        private readonly Vector3 groundPosition = Vector3.zero;

        [Inject] 
        private IBotSpawnFacade _botSpawnFacade;
        [SerializeField] 
        private Transform[] _spawnPositions;
        // TODO: move to the separate file 
        [SerializeField] 
        private List<BotPrefabsData> _botPrefabs;
        [SerializeField] 
        private PlaneInputHandler _planeInputHandler;
        [SerializeField] 
        private Plane.PlaneView _planeView;

        public void Initialize()
        {
            _botSpawnFacade.SpawnBotRequested += OnSpawnBotRequested;
            _botSpawnFacade.GameStarted += OnGameStarted;
            _planeView.Failed += OnFailed;
        }

        public void Dispose()
        {
            _botSpawnFacade.SpawnBotRequested -= OnSpawnBotRequested;
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

        private IEnumerator SpawnCoroutine(BotInfo botInfo)
        {
            yield return new WaitForSeconds(botInfo.SpawnDelay);
            var botPrefab = _botPrefabs.First(item => item.BotType == botInfo.BotType).BotPrefab;
            var pos = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            var bot = Instantiate(botPrefab, null);
            bot.transform.position = pos;
            bot.transform.LookAt(groundPosition);
            bot.BotSpawned += _botSpawnFacade.OnBotSpawned;
            bot.BotDestroyed += _botSpawnFacade.OnBotDestroyed;
            yield return null;
        }
        
        private void OnFailed()
        {
            _botSpawnFacade.OnGameFailed();
        }
    }
}