using System;
using System.Collections.Generic;
using Features.Shared;
using UnityEngine;

namespace Features.Aircraft.Components
{
    public class AircraftBody : MonoBehaviour
    {
        public event Action<Collider> Collision = delegate { };
        
        [SerializeField]
        private Transform _planeModel;
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