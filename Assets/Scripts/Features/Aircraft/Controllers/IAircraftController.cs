using System;
using Features.Aircraft.Components;
using UnityEngine;

namespace Features.Aircraft.Controllers
{
    public interface IAircraftController
    {
        event Action PlaneDestroyed;
        event Action TakeCoin;
        
        bool IsAlive { get; }
        bool IsFirePressed { get; }
        Vector2 MovementState { get; }
        
        void ReportCoinTaken();
        void ReportPlaneDestroyed();
        void InitPlane(AircraftBody aircraftBody);
   //     void SetIsAlive(bool value);
        void DestroyPlane();
    }
}