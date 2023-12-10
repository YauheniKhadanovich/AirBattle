using System;
using UnityEngine;

namespace Features.Aircraft.Components
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