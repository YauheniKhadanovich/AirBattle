using System;
using System.Collections.Generic;
using System.Linq;
using Features.Bots.Impl;
using Features.Environment.Earth.Impl;
using Modules.GameController.Facade;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
        }
        
        private void Update()
        {
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
                _gameControllerFacade.ReportBotDestroyed(botId, byPlayer);
                _gameControllerFacade.DestroyBotsRequested -= bot.FullDamage;
            };
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