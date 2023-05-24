using Features.Bullets;
using UnityEngine;

namespace Features.Plane
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] 
        private FireEffectController _fireEffect;
        [SerializeField] 
        private BulletController _bulletController;
        [SerializeField] 
        private float _fireDelay = 0.3f;

        private float _tmpDelay;

        public void Fire()
        {
            _tmpDelay += Time.deltaTime;

            if (_tmpDelay >= _fireDelay)
            {
                FireProceed();
                _tmpDelay = 0f;
            }
        }

        private void FireProceed()
        {
            var effect = Instantiate(_fireEffect, transform);
            effect.transform.rotation = transform.rotation; // TODO: fix it
            effect.transform.position = transform.position;

            var bullet = Instantiate(_bulletController, null);
            bullet.transform.rotation = transform.rotation; // TODO: fix it
            bullet.transform.position = transform.position;
        }
    }
}