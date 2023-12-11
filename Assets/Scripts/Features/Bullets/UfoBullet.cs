using Features.Environment.Earth;
using UnityEngine;

namespace Features.Bullets
{
    // TODO: refactoring
    public class UfoBullet : BaseBullet
    {
        protected override void Update()
        {
            base.Update();
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, 3f * Time.deltaTime);
        }

        protected override void TimeOver()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IEarth>(out var earth))
            {
                //earth.Hit(_damage);
                Destroy(gameObject);
            }
        }
    }
}