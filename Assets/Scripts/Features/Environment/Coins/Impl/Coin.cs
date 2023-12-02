using System;
using Modules.GameController.Facade;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Features.Environment.Coins.Impl
{
    public class Coin : MonoBehaviour, ICoin, IInitializable, IDisposable
    {
        private const float CoinTime = 7f;

        [Inject]
        private readonly IGameControllerFacade _gameControllerFacade;
        
        [SerializeField] 
        private Transform _coinTransform;
        [SerializeField] 
        private ParticleSystem _takeEffect;
        [SerializeField] 
        private ParticleSystem _expiredEffect;

        private Vector3 _randomRotationDirection;
        private float _rotationSpeed = 50f;
        private float _time = CoinTime;

        public void Initialize()
        {
            _gameControllerFacade.ClearLevelRequested += OnClearLevelRequested;
        }

        public void Dispose()
        {
            _gameControllerFacade.ClearLevelRequested -= OnClearLevelRequested;
        }
        
        private void Start()
        {
            _randomRotationDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            _time = CoinTime;
        }

        public void Take()
        {
            if (_takeEffect)
            {
                var effect = Instantiate(_takeEffect, null);
                effect.transform.position = transform.position;
            }

            Destroy(gameObject);
        }

        private void Expired()
        {
            if (_expiredEffect)
            {
                var effect = Instantiate(_expiredEffect, null);
                effect.transform.position = transform.position;
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            _coinTransform.Rotate(_randomRotationDirection * _rotationSpeed * Time.deltaTime);
            _time -= Time.deltaTime;
            if (_time < 0)
            {
                Expired();
            }
        }
        
        private void OnClearLevelRequested()
        {
            Expired();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}