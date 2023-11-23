using System;
using UnityEngine;

namespace Features.Plane.Components
{
    public class PlaneCollisionsHandler : MonoBehaviour
    {
        public event Action<Collider> OnCollision = delegate { };

        private void OnTriggerEnter(Collider other)
        {
            OnCollision.Invoke(other);
        }
    }
}