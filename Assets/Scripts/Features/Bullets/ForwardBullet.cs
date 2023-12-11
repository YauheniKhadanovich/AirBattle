using Features.Bots;
using UnityEngine;
using UnityEngine.Pool;

namespace Features.Bullets
{
    public class ForwardBullet : BaseBullet
    {
        [SerializeField] 
        private ParticleSystem _impactEffect;

        private ObjectPool<ForwardBullet> _bulletsPool;

        protected override void Update()
        {
            base.Update();
            MoveForward();
        }

        protected override void TimeOver()
        {
            _bulletsPool.Release(this);
        }

        private void DestroySelf()
        {
            var impact = Instantiate(_impactEffect, null);
            impact.transform.position = transform.position;
            
            _bulletsPool.Release(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.Damage(1, true);
                DestroySelf();
            }
        }

        public void SetPool(ObjectPool<ForwardBullet> pool)
        {
            _bulletsPool = pool;
        }
    }
}