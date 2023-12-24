using System;
using Features.Aircraft.Components;
using Features.Aircraft.View;
using Features.Spawner;
using UnityEngine;
using Zenject;

namespace Features.Aircraft.Presenters.Impl
{
    public class AircraftPresenter : IAircraftPresenter, IInitializable, IDisposable, ITickable
    {
        private readonly IAircraftView _aircraftView;
        private readonly IGameSpawner _gameSpawner;

        private PlayerInput _playerInput;
        
        private Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        private bool IsFirePressed => _playerInput.Player.Fire.IsPressed();
        public bool IsAlive { get; private set; }

        public AircraftPresenter(IAircraftView aircraftView, IGameSpawner gameSpawner)
        {
            _aircraftView = aircraftView ?? throw new ArgumentNullException(nameof(aircraftView));
            _gameSpawner = gameSpawner ?? throw new ArgumentNullException(nameof(gameSpawner));
        }

        public void Initialize()
        {
            _aircraftView.AircraftDestroyed += OnAircraftDestroyed;
            _aircraftView.CoinTaken += OnCoinTaken;
            _gameSpawner.GameFailed += OnGameFailed;
            _gameSpawner.GameStarted += OnGameStarted;
            
            _playerInput = new PlayerInput();
            _playerInput.Player.Enable();
            IsAlive = false;
        }

        public void Dispose()
        {
            _aircraftView.AircraftDestroyed -= OnAircraftDestroyed;
            _aircraftView.CoinTaken -= OnCoinTaken;
            _gameSpawner.GameFailed -= OnGameFailed;
        }

        public void Tick()
        {
            _aircraftView.MoveForward();         
            if (!IsAlive)
            {
                return;
            }
            
            _aircraftView.ControlPlane(MovementState);
            if (IsFirePressed)
            {
                _aircraftView.Fire();
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
            _aircraftView.SetBody(aircraftBody);
        }
        
        private void OnGameFailed()
        {
            if (IsAlive)
            {
                IsAlive = false;
                _aircraftView.DestroyAircraft();
            } 
        }
    }
}