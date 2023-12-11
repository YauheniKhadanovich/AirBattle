using Features.Aircraft.Components;
using Features.Aircraft.Controllers;
using Features.Bots;
using Features.Environment.Coins;
using Features.Shared;
using Features.Spawner;
using UnityEngine;
using Zenject;

namespace Features.Aircraft.View.Impl // TODO
{
    public class PlaneView : CanFly, IPlaneView
    {
        [Inject] 
        private IAircraftController _aircraftController;
        [Inject]
        private IObjectPoolController _objectPoolController;
        
        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField]
        private Transform _bodySpawnPosition;

        private float _xParam;
        private AircraftBody _aircraftBody = null;

        private void Update()
        {
            MoveForward();
            ControlPlane();
            Fire();
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
        
        public void DestroyPlane()
        {
            if (_aircraftController.IsAlive)
            {
                DestroySelf();
            }
        }
        
        private void Fire()
        {
            if (!_aircraftController.IsFirePressed || !_aircraftController.IsAlive)
            {
                return;
            }

            if (_aircraftBody)
            {
                _aircraftBody.Fire();
            }
        }
        
        private void ControlPlane()
        {
            if (!_aircraftController.IsAlive)
            {
                return;
            }
            
            var localEulerAngles = _bodyDirection.localEulerAngles;
            _xParam = Mathf.Lerp(_xParam, _aircraftController.MovementState.x, Time.deltaTime * 5f);
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
            if (!_aircraftController.IsAlive)
            {
                return;
            }
            
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.FullDamage();
                DestroySelf();
            }
            if (other.gameObject.TryGetComponent<ICoin>(out var coin))
            {
                _aircraftController.ReportCoinTaken();
                coin.Take();
            }
        }

        private void DestroySelf()
        {
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            if (_aircraftBody)
            {
                _aircraftBody.Collision -= OnCollision;
                Destroy(_aircraftBody.gameObject);
                _aircraftBody = null;
            }
            _aircraftController.ReportPlaneDestroyed();
        }
    }
}