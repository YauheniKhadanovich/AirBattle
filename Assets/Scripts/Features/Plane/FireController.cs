using Features.Bullets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Plane
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] 
        private FireEffectController _fireEffect;
        [FormerlySerializedAs("_bulletController")] [SerializeField] 
        private Bullet bullet;
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
            var effect = Instantiate(_fireEffect, transform);
            effect.transform.rotation = transform.rotation; // TODO: fix it
            effect.transform.position = transform.position;

            var bullet = Instantiate(this.bullet, null);
            bullet.transform.rotation = transform.rotation; // TODO: fix it
            bullet.transform.position = transform.position;
        }
    }
}