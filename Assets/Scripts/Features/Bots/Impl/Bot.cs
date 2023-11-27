using System;
using Features.Shared;
using Features.Spawner;
using Modules.GameController.Facade;
using UnityEngine;
using Zenject;

namespace Features.Bots.Impl
{
    public class Bot : CanFly, IMortal, IInitializable, IDisposable
    {
        [Inject]
        private readonly IGameControllerFacade _gameControllerFacade;
        [Inject]
        private readonly IGameSpawner _gameSpawner;
        
        [SerializeField]
        private bool _isContainsCoin;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField]
        private ParticleSystem _smokeParticle;
        [SerializeField] 
        private int _health = 10;

        private int _currentHealth;
        private string _botId;

        protected bool IsAlive => _currentHealth > 0;

        public void Initialize()
        {
            _gameControllerFacade.ClearLevelRequested += OnClearLevelRequested;
        }
        
        public void Dispose()
        {
            _gameControllerFacade.ClearLevelRequested -= OnClearLevelRequested;
        }
        
        public void SetData(string botId)
        {
            _botId = botId;
            _currentHealth = _health;
        }
        
        public void Damage(int damageValue, bool byPlayer)
        {
            _currentHealth -= damageValue;
            if (!IsAlive)
            {
                Destroy(byPlayer);
            }
            else
            {
                if (_smokeParticle && !_smokeParticle.isPlaying)
                {
                    _smokeParticle.Play();
                }
            }
        }

        public void FullDamage()
        {
            Destroy(false);
        }

        private void OnClearLevelRequested()
        {
            FullDamage();
        }
        
        private void Destroy(bool byPlayer)
        {
            if (_isContainsCoin && byPlayer)
            {
                _gameSpawner.SpawnCoin(transform.position);
            }
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            _gameControllerFacade.Bots[_botId].ReduceSpawnedBotsCount();
            _gameControllerFacade.ReportBotDestroyed(_botId, byPlayer);
            GameObject.Destroy(gameObject);
        }
        
        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}