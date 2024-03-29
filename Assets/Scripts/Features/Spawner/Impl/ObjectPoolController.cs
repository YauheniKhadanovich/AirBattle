using System.Collections;
using Features.Bullets;
using UnityEngine;
using UnityEngine.Pool;

namespace Features.Spawner.Impl
{
    public class ObjectPoolController : MonoBehaviour, IObjectPoolController
    {
        private ObjectPool<ForwardBullet> _bulletsPool;
        private ObjectPool<ParticleSystem> _bulletsImpactEffectPool;

        [SerializeField] 
        private ForwardBullet _bullet;
        [SerializeField] 
        private ParticleSystem _bulletImpactEffect;

        private void Start()
        {
            _bulletsPool = new ObjectPool<ForwardBullet>(CreateBullet, OnTakeBullet, OnReturnBullet, OnDestroyBullet, true, 10, 10);
            _bulletsImpactEffectPool = new ObjectPool<ParticleSystem>(CreateBulletImpactEffect, OnTakeBulletImpactEffect, OnReturnBulletImpactEffect, OnDestroyBulletImpactEffect, true, 4, 4);
        }

        public void SpawnBullet(Quaternion rotation, Vector3 position)
        {
            SpawnBulletInternal(rotation, position);
        }

        #region ImpactEffect

        private void OnDestroyBulletImpactEffect(ParticleSystem effect)
        {
            Destroy(effect.gameObject);
        }

        private void OnReturnBulletImpactEffect(ParticleSystem effect)
        {
            effect.gameObject.SetActive(false);
        }

        private void OnTakeBulletImpactEffect(ParticleSystem effect)
        {
            effect.gameObject.SetActive(true);
        }

        private ParticleSystem CreateBulletImpactEffect()
        {
            var impact = Instantiate(_bulletImpactEffect);
            impact.transform.position = transform.position;
            impact.gameObject.SetActive(false);
            return impact;
        }

        private void SpawnBulletImpactEffect(Vector3 pos)
        {
            var effect = _bulletsImpactEffectPool.Get();
            effect.transform.position = pos;
            effect.gameObject.SetActive(true);
            effect.Play();
            StartCoroutine(ReturnBulletImpactEffectAfterCompletion(effect));
        }

        private IEnumerator ReturnBulletImpactEffectAfterCompletion(ParticleSystem effect)
        {
            yield return new WaitForSeconds(effect.main.duration);
            _bulletsImpactEffectPool.Release(effect);
        }

        #endregion

        #region Bullet

        private void SpawnBulletInternal(Quaternion rot, Vector3 pos)
        {
            var bullet = _bulletsPool.Get();

            bullet.transform.position = pos;
            bullet.transform.rotation = rot;
        }

        private void ReleaseBullet(ForwardBullet bullet)
        {
            _bulletsPool.Release(bullet);
        }

        private ForwardBullet CreateBullet()
        {
            var bullet = Instantiate(_bullet);
            bullet.Disabled += ReleaseBullet;
            bullet.EffectRequested += SpawnBulletImpactEffect;
            return bullet;
        }

        private void OnTakeBullet(ForwardBullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnReturnBullet(ForwardBullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

        private void OnDestroyBullet(ForwardBullet bullet)
        {
            bullet.Disabled -= ReleaseBullet;
            bullet.EffectRequested -= SpawnBulletImpactEffect;
            Destroy(bullet.gameObject);
        }

        #endregion
    }
}