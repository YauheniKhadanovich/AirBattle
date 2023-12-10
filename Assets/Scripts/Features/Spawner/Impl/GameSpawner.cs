using System;
using System.Collections.Generic;
using System.Linq;
using Features.Aircraft.Controllers;
using Features.Bots.Impl;
using Features.Environment.Coins.Impl;
using Features.Environment.Earth.Impl;
using Modules.GameController.Facade;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Spawner.Impl
{
    public class GameSpawner : MonoBehaviour, IGameSpawner, IInitializable, IDisposable
    {
        private readonly Vector3 _groundPosition = Vector3.zero;

        [Inject] 
        private readonly IGameControllerFacade _gameControllerFacade;
        [Inject]
        private readonly DiContainer _container;
        [Inject]
        private readonly IAircraftController _aircraftController;
        
        [SerializeField] 
        private Coin _coin;
        [SerializeField] 
        private Earth _earth;
        [SerializeField] 
        private Volume _volume;

        private List<Transform> _spawnPositions;

        public void Initialize()
        {
            _gameControllerFacade.GameStarted += OnGameStarted;
            _gameControllerFacade.GameFailed += OnGameFailed;
            _aircraftController.PlaneDestroyed += OnPlaneDestroyed;
            _aircraftController.TakeCoin += OnTakeCoin;
        }

        public void Dispose()
        {
            _gameControllerFacade.GameStarted -= OnGameStarted;
            _gameControllerFacade.GameFailed -= OnGameFailed;
            _aircraftController.PlaneDestroyed -= OnPlaneDestroyed;
            _aircraftController.TakeCoin -= OnTakeCoin;
        }

        private void Start()
        {
            _spawnPositions = GetComponentsInChildren<SpawnPoint>().Select(point => point.transform).ToList();
        }

        private void Update()
        {
            ChangeVolume();
            if (!_aircraftController.IsAlive)
            {
                return;
            }
            foreach (var botInfo in _gameControllerFacade.Bots.Select(pair => pair.Value))
            {

                if (!botInfo.IsNeedSpawnByCount)
                {
                    continue;
                }
                botInfo.TimerTick(Time.deltaTime);
                
                if (!botInfo.IsNeedSpawnByTime)
                {
                    continue;
                }

                SpawnBot(botInfo.BotId, botInfo.BotPrefab);
                botInfo.Spawned();
            }
        }

        private void SpawnBot(string botId, Bot botPrefab)
        {
            var position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
            var bot = _container.InstantiatePrefabForComponent<Bot>(botPrefab, position, Quaternion.identity, null);
                          
            if (bot is IInitializable initializableBot)
            {
                initializableBot.Initialize();
            }
            
            bot.SetData(botId);
            bot.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
            bot.transform.LookAt(_groundPosition);
        }

        public void SpawnCoin(Vector3 position)
        {
            var coin = _container.InstantiatePrefabForComponent<Coin>(_coin, position, Quaternion.identity, null);
            if (coin is IInitializable initializableCoin)
            {
                initializableCoin.Initialize();
            }
        }
        
        private void OnGameStarted()
        {
            var aircraftBodyPrefab = _gameControllerFacade.GetCurrentAircraftBodyPrefab();
            _aircraftController.InitPlane(aircraftBodyPrefab);
        }
        
        private void OnPlaneDestroyed()
        {
            _gameControllerFacade.ReportPlayerDestroyed();
        }
        
        private void OnTakeCoin()
        {
            _gameControllerFacade.ReportCoinTaken();
        }
        
        private void OnGameFailed()
        {
            _aircraftController.DestroyPlane();
        }

        private void ChangeVolume()
        {
            float targetValue = _aircraftController.IsAlive || _gameControllerFacade.GameInProgress ? 50 : -100;

            _volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
            var currentValue = colorAdjustments.saturation.value;

            colorAdjustments.saturation.value = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 7.4f);
        }
    }
}