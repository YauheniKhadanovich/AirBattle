using System;
using Features.Aircraft.Components;
using Features.Aircraft.View;
using Features.Spawner;
using UnityEngine;
using Zenject;

namespace Features.Aircraft.Controllers.Impl
{
    public class AircraftPresenter : IAircraftPresenter, IInitializable, IDisposable, ITickable
    {
        private readonly IPlaneView _planeView;
        private readonly IGameSpawner _gameSpawner;

        private PlayerInput _playerInput;
        
        private Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        private bool IsFirePressed => _playerInput.Player.Fire.IsPressed();
        public bool IsAlive { get; private set; }

        public AircraftPresenter(IPlaneView planeView, IGameSpawner gameSpawner)
        {
            _planeView = planeView ?? throw new ArgumentNullException(nameof(planeView));
            _gameSpawner = gameSpawner ?? throw new ArgumentNullException(nameof(gameSpawner));
        }

        public void Initialize()
        {
            _planeView.AircraftDestroyed += OnAircraftDestroyed;
            _planeView.CoinTaken += OnCoinTaken;
            _gameSpawner.GameFailed += OnGameFailed;
            _gameSpawner.GameStarted += OnGameStarted;
            
            _playerInput = new PlayerInput();
            _playerInput.Player.Enable();
            IsAlive = false;
        }

        public void Dispose()
        {
            _planeView.AircraftDestroyed -= OnAircraftDestroyed;
            _planeView.CoinTaken -= OnCoinTaken;
            _gameSpawner.GameFailed -= OnGameFailed;
        }

        public void Tick()
        {
            _planeView.MoveForward();         
            if (!IsAlive)
            {
                return;
            }
            
            _planeView.ControlPlane(MovementState);
            if (IsFirePressed)
            {
                _planeView.Fire();
            }
        }
        
        private void OnCoinTaken()
        {
            _gameSpawner.ReportCoinTaken();
        }
        
        private void OnAircraftDestroyed()
        {
            IsAlive = false;
            _gameSpawner.ReportAircraftDestroyed();
        }
        
        private void OnGameStarted(AircraftBody aircraftBody)
        {
            IsAlive = true;
            _planeView.SetBody(aircraftBody);
        }
        
        private void OnGameFailed()
        {
            if (IsAlive)
            {
                IsAlive = false;
                _planeView.DestroyAircraft();
            } 
        }
    }
}