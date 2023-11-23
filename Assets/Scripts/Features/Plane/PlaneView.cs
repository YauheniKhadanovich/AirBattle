using System;
using System.Collections.Generic;
using Features.Bots;
using Features.Environment.Coins;
using Features.Plane.Components;
using Features.Shared;
using UnityEngine;

namespace Features.Plane
{
    public class PlaneView : CanFly // TODO: add zenject initializer
    {
        public event Action PlaneDestroyed =  delegate { };
        public event Action TakeCoin =  delegate { };

        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField] 
        private List<FireController> _fireControllers;
        [SerializeField] 
        private PlaneInputHandler _planeInputHandler;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField]
        private Transform _planeModel;
        [SerializeField]
        private PlaneCollisionsHandler _collisionsHandler;
      
        private float _xParam;

        public bool IsAlive { get; private set; }

        private void Start()
        {
            _collisionsHandler.OnCollision += OnCollision;
            IsAlive = false;
            _planeModel.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            MoveForward();
            ControlPlane();
            FireIfNeed();
        }

        private void OnDestroy()
        {
            _collisionsHandler.OnCollision -= OnCollision;
        }
        
        public void InitPlane()
        {
            IsAlive = true;
            _planeModel.gameObject.SetActive(true);
        }

        public void DestroyPlane()
        {
            if (IsAlive)
            {
                DestroySelf();
            }
        }
        
        private void FireIfNeed()
        {
            if (!_planeInputHandler.IsFirePressed || !IsAlive)
            {
                return;
            }

            _fireControllers.ForEach(item=>item.Fire());
        }
        
        private void ControlPlane()
        {
            if (!IsAlive)
            {
                return;
            }
            
            var localEulerAngles = _bodyDirection.localEulerAngles;
            _xParam = Mathf.Lerp(_xParam, _planeInputHandler.MovementState.x, Time.deltaTime * 5f);
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
            if (!IsAlive)
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
                TakeCoin.Invoke();
                coin.Take();
            }
        }

        private void DestroySelf()
        {
            IsAlive = false;
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            _planeModel.gameObject.SetActive(false);
            PlaneDestroyed.Invoke();
        }
    }
}