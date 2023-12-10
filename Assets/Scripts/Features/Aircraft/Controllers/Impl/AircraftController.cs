using System;
using Features.Aircraft.Components;
using Features.Aircraft.View;
using UnityEngine;
using Zenject;

namespace Features.Aircraft.Controllers.Impl
{
    public class AircraftController : IAircraftController, IInitializable, IDisposable
    {
        public event Action PlaneDestroyed = delegate { };
        public event Action TakeCoin = delegate { };
        
        [Inject] 
        private readonly IPlaneView _planeView;

        private PlayerInput _playerInput;
        
        public Vector2 MovementState => _playerInput.Player.Move.ReadValue<Vector2>();
        public bool IsFirePressed => _playerInput.Player.Fire.IsPressed();
        public bool IsAlive { get; private set; }

        public void Initialize()
        {
            _playerInput = new PlayerInput();
            _playerInput.Player.Enable();
            IsAlive = false;
        }

        public void Dispose()
        {
//
        }

        public void ReportCoinTaken()
        {
            TakeCoin.Invoke();
        }

        public void ReportPlaneDestroyed()
        {
            IsAlive = false;
            PlaneDestroyed.Invoke();
        }

        public void InitPlane(AircraftBody aircraftBody)
        {
            IsAlive = true;
            _planeView.SetBody(aircraftBody);
        }

        public void DestroyPlane()
        {
            _planeView.DestroyPlane(); 
        }
    }
}