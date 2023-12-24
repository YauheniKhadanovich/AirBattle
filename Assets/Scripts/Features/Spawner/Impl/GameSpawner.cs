using System;
using System.Collections.Generic;
using System.Linq;
using Features.Aircraft.Components;
using Features.Bots.Impl;
using Features.Environment.Coins.Impl;
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
        public event Action GameFailed = delegate { };
        public event Action<AircraftBody> GameStarted = delegate { };
        
        private Vector3 _groundPosition = Vector3.zero;
        private IGameControllerFacade _gameControllerFacade;
        private DiContainer _container;

        [SerializeField] 
        private Coin _coin;
        [SerializeField] 
        private Volume _volume;

        private List<Transform> _spawnPositions;
        private bool _needSpawn;
        private float _currentSaturationValue = 50;
    
        [Inject]
        public void Construct(IGameControllerFacade gameControllerFacade, DiContainer container)
        {
            _gameControllerFacade = gameControllerFacade ?? throw new ArgumentNullException(nameof(gameControllerFacade));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Initialize()
        {
            _gameControllerFacade.GameStarted += OnGameStarted;
            _gameControllerFacade.GameFailed += OnGameFailed;
        }

        public void Dispose()
        {
            _gameControllerFacade.GameStarted -= OnGameStarted;
            _gameControllerFacade.GameFailed -= OnGameFailed;
        }

        private void Start()
        {
            _spawnPositions = GetComponentsInChildren<SpawnPoint>().Select(point => point.transform).ToList();
        }
        
        public void ReportAircraftDestroyed()
        {
            _gameControllerFacade.ReportPlayerDestroyed();
        }

        public void ReportCoinTaken()
        {
            _gameControllerFacade.ReportCoinTaken();
        }
        
        public void SpawnCoin(Vector3 position)
        {
            var coin = _container.InstantiatePrefabForComponent<Coin>(_coin, position, Quaternion.identity, null);
            if (coin is IInitializable initializableCoin)
            {
                initializableCoin.Initialize();
            }
        }

        private void Update()
        {
            ChangeVolume();
            if (!_needSpawn)
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
        
        private void OnGameStarted()
        {
            _currentSaturationValue = 50;
            _needSpawn = true;
            GameStarted.Invoke(_gameControllerFacade.GetCurrentAircraftBodyPrefab());
        }
        
        private void OnGameFailed()
        {
            _currentSaturationValue = -100;
            _needSpawn = false;
            GameFailed.Invoke();
        }

        private void ChangeVolume()
        {
            _volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
            var currentValue = colorAdjustments.saturation.value;

            colorAdjustments.saturation.value = Mathf.Lerp(currentValue, _currentSaturationValue, Time.deltaTime * 7.4f);
        }
    }
}