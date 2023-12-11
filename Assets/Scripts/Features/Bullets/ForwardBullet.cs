using Features.Bots;
using Features.Spawner;
using UnityEngine;

namespace Features.Bullets
{
    public class ForwardBullet : BaseBullet
    {
        private IObjectPoolController _objectPoolController;

        protected override void Update()
        {
            base.Update();
            MoveForward();
        }
        
        public void SetPoolController(IObjectPoolController poolController)
        {
            _objectPoolController = poolController;
        }

        protected override void TimeOver()
        {
            _objectPoolController.ReleaseBullet(this);
        }

        private void DestroySelf()
        {
            _objectPoolController.SpawnBulletImpactEffect(transform.position);
            _objectPoolController.ReleaseBullet(this);
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