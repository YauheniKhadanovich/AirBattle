using System;
using Features.Aircraft.Components;
using UnityEngine;

namespace Features.Spawner
{
    public interface IGameSpawner
    {
        event Action GameFailed;
        event Action<AircraftBody> GameStarted;
            
        void SpawnCoin(Vector3 position);
        void ReportAircraftDestroyed();
        void ReportCoinTaken();
    }
}