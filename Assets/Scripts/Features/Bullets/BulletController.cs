using UnityEngine;

namespace Features.Bullets
{
    public class BulletController : CanFlyController
    {
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
                DestroySelf();
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}