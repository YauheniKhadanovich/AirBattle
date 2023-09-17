using Features.Shared;
using UnityEngine;

namespace Features.Bullets
{
    public class BaseBullet : CanFly
    {
        [SerializeField] 
        private float _lifeTime = 4f;

        private float _lifeTimeTmp;

        protected virtual void Update()
        {
            CheckLifeTime();
        }
        
        private void CheckLifeTime()
        {
            _lifeTimeTmp += Time.deltaTime;
            if (_lifeTimeTmp > _lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}