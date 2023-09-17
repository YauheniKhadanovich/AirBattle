using Features.Bullets;
using UnityEngine;

namespace Features.Plane.Components
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem _fireEffect;
        [SerializeField] 
        private BaseBullet _bullet;
        [SerializeField] 
        private float _fireDelay = 0.15f;

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
            if (_fireEffect)
            {
                var effect = Instantiate(_fireEffect, transform);
                effect.transform.rotation = transform.rotation;
                effect.transform.position = transform.position;
            }

            if (_bullet)
            {
                var bullet = Instantiate(_bullet, null);
                bullet.transform.rotation = transform.rotation;
                bullet.transform.position = transform.position;
            }
        }
    }
}