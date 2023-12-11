using System;
using System.Collections.Generic;
using Features.Shared;
using Features.Spawner;
using Features.Spawner.Impl;
using UnityEngine;

namespace Features.Aircraft.Components
{
    public class AircraftBody : MonoBehaviour
    {
        public event Action<Collider> Collision = delegate { };
        
        [SerializeField] 
        private List<FireController> _fireControllers;
        [SerializeField]
        private PlaneCollisionsHandler _collisionsHandler;
        
        private void Start()
        {
            _collisionsHandler.OnCollision += OnCollision;
        }

        private void OnDestroy()
        {
            _collisionsHandler.OnCollision -= OnCollision;
        }
        
        public void SetPoolManager(IObjectPoolController objectPoolController)
        {
            _fireControllers.ForEach(item => item.SetPoolController(objectPoolController));

        }
        
        public void Fire()
        {
            _fireControllers.ForEach(item => item.Fire());
        }
        
        private void OnCollision(Collider coll)
        {
            Collision.Invoke(coll);
        }
    }
}