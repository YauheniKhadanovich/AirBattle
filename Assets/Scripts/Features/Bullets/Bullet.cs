using Features.Bots;
using UnityEngine;

namespace Features.Bullets
{
    public class Bullet : CanFly
    {
        [SerializeField] 
        private ParticleSystem _impactEffect;
        [SerializeField] 
        private float _lifeTime = 4f;
        private float _lifeTimeTmp;

        private void Update()
        {
            CheckLifeTime();
            MoveForward();
        }

        private void CheckLifeTime()
        {
            _lifeTimeTmp += Time.deltaTime;
            if (_lifeTimeTmp > _lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void DestroySelf()
        {
            var impact = Instantiate(_impactEffect, null);
            impact.transform.position = transform.position;
            
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<IMortal>(out var obj))
            {
                obj.Damage(1); // TODO: add damage value
                DestroySelf();
            }
        }
    }
}