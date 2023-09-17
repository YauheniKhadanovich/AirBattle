using Features.Earth;
using UnityEngine;

namespace Features.Bullets
{
    public class UfoBullet : BaseBullet
    {
        [SerializeField]
        private float _damage = 10;
        
        protected override void Update()
        {
            base.Update();
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 3f * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IEarth>(out var earth))
            {
                earth.Hit(_damage);
                Destroy(gameObject);
            }
        }
    }
}