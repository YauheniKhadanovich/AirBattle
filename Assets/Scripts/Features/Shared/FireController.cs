using Features.Spawner;
using UnityEngine;

namespace Features.Shared
{
    public class FireController : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem _fireEffect;
        [SerializeField] 
        private float _fireDelay = 0.15f;

        private IObjectPoolController _objectPoolController;

        private float _tmpDelay;

        public void SetPoolController(IObjectPoolController objectPoolController)
        {
            _objectPoolController = objectPoolController;
        }
        
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

            _objectPoolController.SpawnBullet(transform.rotation, transform.position);
        }
    }
}