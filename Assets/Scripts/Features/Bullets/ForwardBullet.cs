using Features.Bots;
using UnityEngine;

namespace Features.Bullets
{
    public class ForwardBullet : BaseBullet
    {
        [SerializeField] 
        private ParticleSystem _impactEffect;
        
        protected override void Update()
        {
            base.Update();
            MoveForward();
        }
        
        private void DestroySelf()
        {
            var impact = Instantiate(_impactEffect, null);
            impact.transform.position = transform.position;
            
            Destroy(gameObject);
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