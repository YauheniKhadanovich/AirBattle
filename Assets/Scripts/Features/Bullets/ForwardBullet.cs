using System;
using Features.Bots;
using UnityEngine;

namespace Features.Bullets
{
    public class ForwardBullet : BaseBullet
    {
        public event Action<ForwardBullet> Disabled = delegate { }; 
        public event Action<Vector3> EffectRequested = delegate { }; 

        protected override void Update()
        {
            base.Update();
            MoveForwardInternal();
        }
        
        protected override void TimeOver()
        {
            Disabled.Invoke(this);
        }

        private void DestroySelf()
        {
            EffectRequested.Invoke(transform.position);
            Disabled.Invoke(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.Damage(1, true);
                DestroySelf();
            }
        }
    }
}