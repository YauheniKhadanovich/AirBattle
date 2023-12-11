using UnityEngine;

namespace Features.Spawner
{
    public interface IObjectPoolController
    {
        void SpawnBullet(Quaternion rot, Vector3 pos);
    }
}