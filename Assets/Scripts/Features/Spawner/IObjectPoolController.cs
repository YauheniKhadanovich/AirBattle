using Features.Bullets;
using UnityEngine;

namespace Features.Spawner
{
    public interface IObjectPoolController
    {
        void SpawnBullet(Quaternion rot, Vector3 pos);

        void SpawnBulletImpactEffect(Vector3 pos);
        void ReleaseBullet(ForwardBullet bullet);
    }
}