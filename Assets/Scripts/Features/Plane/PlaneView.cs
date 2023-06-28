using System;
using Features.Bots;
using Features.Plane.Components;
using Features.Shared;
using UnityEngine;

namespace Features.Plane
{
    public class PlaneView : CanFly
    {
        public event Action Failed =  delegate { };

        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField] 
        private FireController _fireController;
        [SerializeField] 
        private PlaneInputHandler _planeInputHandler;
        [SerializeField]
        private ParticleSystem _destroyParticle;
        [SerializeField]
        private Transform _planeModel;
        [SerializeField]
        private PlaneCollisionsController _collisionsController;
        private float xParam;
        private bool _isAlive;

        private void Start()
        {
            _collisionsController.OnCollision += OnCollision;
        }
        
        private void Update()
        {
            MoveForward();
            ControlPlane();
            FireIfNeed();
        }

        private void OnDestroy()
        {
            _collisionsController.OnCollision -= OnCollision;
        }
        
        public void InitPlane()
        {
            _isAlive = true;
            _planeModel.gameObject.SetActive(true);
        }

        private void FireIfNeed()
        {
            if (!_planeInputHandler.IsFirePressed || !_isAlive)
            {
                return;
            }

            _fireController.Fire();
        }

        // TODO: use consts
        private void ControlPlane()
        {
            if (!_isAlive)
            {
                return;
            }
            
            var localEulerAngles = _bodyDirection.localEulerAngles;
            xParam = Mathf.Lerp(xParam, _planeInputHandler.MovementState.x, Time.deltaTime * 5f);
            var targetEulerAngles = new Vector3(0f, localEulerAngles.y + xParam * 40f, 0f);
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
            if (!_isAlive)
            {
                return;
            }
            
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.FullDamage(); // TODO: add damage value
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            _isAlive = false;
            var particle = Instantiate(_destroyParticle, null);
            particle.transform.position = transform.position;
            _planeModel.gameObject.SetActive(false);
            Failed.Invoke();
        }
    }
}