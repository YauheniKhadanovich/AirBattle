using UnityEngine;

namespace Features.Spawner
{
    public interface IObjectPoolController
    {
        void SpawnBullet(Quaternion rotation, Vector3 position);
    }
}