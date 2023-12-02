using System;
using System.Collections.Generic;
using System.Linq;
using Features.Bots.Impl;
using Features.Environment.Coins.Impl;
using Features.Environment.Earth.Impl;
using Features.UI.ViewManagement;
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
        private readonly IViewManager _viewManager;
        [Inject]
        private readonly DiContainer _container;

        [SerializeField] 
        private Coin _coin;
        [SerializeField] 
        private List<Transform> _spawnPositions;
        [SerializeField] 
        private Plane.PlaneView _planeView;
        [SerializeField] 
        private Earth _earth;
        [SerializeField] 
        private Volume _volume;

        // TODO: remove it
        private bool _isFirstGame = true;
        
        public void Initialize()
        {
            _gameControllerFacade.GameStarted += OnGameStarted;
            _gameControllerFacade.GameFailed += OnGameFailed;
            _planeView.PlaneDestroyed += OnPlaneDestroyed;
            _planeView.TakeCoin += OnTakeCoin;
        }

        public void Dispose()
        {
            _gameControllerFacade.GameStarted -= OnGameStarted;
            _gameControllerFacade.GameFailed -= OnGameFailed;
            _planeView.PlaneDestroyed -= OnPlaneDestroyed;
            _planeView.TakeCoin -= OnTakeCoin;
        }
        
        private void Update()
        {
            if (Input.GetKeyUp("a"))
            {
                _viewManager.OpenFailView();
            }
            
            ChangeVolume();
            foreach (var botInfo in _gameControllerFacade.Bots.Select(pair => pair.Value))
            {
                if (!_planeView.IsAlive)
                {
                    return;
                }
                if (!botInfo.IsNeedSpawnByCount)
                {
                    continue;
                }
                botInfo.TimerTick(Time.deltaTime);
                
                if (!botInfo.IsNeedSpawnByTime)
                {
                    continue;
                }
                
                SpawnBot(botInfo.BotId, botInfo.BotTo.BotPrefab);
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
            _isFirstGame = false;
            _planeView.InitPlane();
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
            _planeView.DestroyPlane();
        }

        private void ChangeVolume()
        {
            float targetValue = _planeView.IsAlive || _isFirstGame ? 50 : -100;

            _volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
            var currentValue = colorAdjustments.saturation.value;

            colorAdjustments.saturation.value = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 8f);
        }
    }
}