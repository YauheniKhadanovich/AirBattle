using System;
using Features.Aircraft.Components;
using Features.Bots;
using Features.Environment.Coins;
using Features.Shared;
using Features.Spawner;
using UnityEngine;
using Zenject;


namespace Features.Aircraft.View.Impl
{
    public class AircraftView: CanFly, IAircraftView
    {
        public event Action AircraftDestroyed = delegate { };
        public event Action CoinTaken = delegate { };

        private IObjectPoolController _objectPoolController;

        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField] 
        private ParticleSystem _destroyParticle;
        [SerializeField] 
        private Transform _bodySpawnPosition;

        private float _xParam;
        private AircraftBody _aircraftBody;

        [Inject]
        public void Construct(IObjectPoolController objectPoolController)
        {
            _objectPoolController = objectPoolController ?? throw new ArgumentNullException(nameof(objectPoolController));
        }

        public void SetBody(AircraftBody aircraftBody)
        {
            if (_aircraftBody)
            {
                _aircraftBody.Collision -= OnCollision;
                Destroy(_aircraftBody.gameObject);
                _aircraftBody = null;
            }

            _aircraftBody = Instantiate(aircraftBody, _bodySpawnPosition.position, _bodySpawnPosition.rotation, _bodySpawnPosition);
            _aircraftBody.SetPoolManager(_objectPoolController);
            _aircraftBody.Collision += OnCollision;
        }


        public void DestroyAircraft()
        {
            DestroyAircraftInternal();
        }

        public void Fire()
        {
            if (_aircraftBody)
            {
                _aircraftBody.Fire();
            }
        }

        public void MoveForward()
        {
            MoveForwardInternal();
        }

        public void ControlPlane(Vector2 movementState)
        {
            var localEulerAngles = _bodyDirection.localEulerAngles;
            _xParam = Mathf.Lerp(_xParam, movementState.x, Time.deltaTime * 5f);
            var targetEulerAngles = new Vector3(0f, localEulerAngles.y + _xParam * 40f, 0f);
            localEulerAngles = Vector3.Lerp(localEulerAngles, targetEulerAngles, Time.deltaTime * 2f);
            _bodyDirection.localEulerAngles = localEulerAngles;
            Tilt(targetEulerAngles);
        }

        private void Tilt(Vector3 targetEulerAngles)
        {
            var tiltAngle = Mathf.Clamp(Mathf.DeltaAngle(targetEulerAngles.y, _bodyDirection.localEulerAngles.y) * 1.5f, -89f, 89f);
            _bodyRotation.localEulerAngles = new Vector3(-tiltAngle, 0, 0);
        }

        private void OnCollision(Collider other)
        {
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.FullDamage(); // TODO: refactoring
                DestroyAircraftInternal();
                AircraftDestroyed.Invoke();
            }

            if (other.gameObject.TryGetComponent<ICoin>(out var coin))
            {
                CoinTaken.Invoke();
                coin.Take();
            }
        }

        private void DestroyAircraftInternal()
        {
            if (!_aircraftBody)
            {
                return;
            }
            
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            _aircraftBody.Collision -= OnCollision;
            Destroy(_aircraftBody.gameObject);
            _aircraftBody = null;
        }
    }
}