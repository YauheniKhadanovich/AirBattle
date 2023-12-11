using Features.Bullets;
using UnityEngine;
using UnityEngine.Pool;

namespace Features.Spawner.Impl
{
    public class ObjectPoolController : MonoBehaviour, IObjectPoolController
    {
        private ObjectPool<ForwardBullet> _bulletsPool;

        [SerializeField]
        private ForwardBullet _bullet; // TODO: refactoring
        
        private void Start()
        {
            _bulletsPool = new ObjectPool<ForwardBullet>(CreateBullet, OnTake, OnReturn, OnDestroyBullet, true, 10, 10);
        }
        
        public void SpawnBullet(Quaternion rot, Vector3 pos)
        {
            var bullet = _bulletsPool.Get();

            bullet.transform.position = pos;
            bullet.transform.rotation = rot;
        }

        private ForwardBullet CreateBullet()
        {
            var bullet = Instantiate(_bullet);
            bullet.SetPool(_bulletsPool);
            return bullet;
        }

        private void OnTake(ForwardBullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }
        
        private void OnReturn(ForwardBullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
        
        private void OnDestroyBullet(ForwardBullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }
    }
}