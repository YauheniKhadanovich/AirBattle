using System;
using UnityEngine;

namespace Features.Plane.Components
{
    public class PlaneCollisionsController : MonoBehaviour
    {
        public event Action<Collider> OnCollision = delegate { };
        
        private void OnTriggerEnter(Collider other)
        {
        //    OnCollision.Invoke(other);
        }
    }
}