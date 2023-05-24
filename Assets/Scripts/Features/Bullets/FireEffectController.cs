using UnityEngine;

namespace Features.Bullets
{
    public class FireEffectController : MonoBehaviour
    {
        private readonly float _lifeTime = 3f;
        
        private float _lifeTimeTmp;
        
        private void Update()
        {
            CheckLifeTime();
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