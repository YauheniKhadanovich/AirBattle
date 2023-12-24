using System;
using Features.Aircraft.Components;
using UnityEngine;

namespace Features.Aircraft.View
{
    public interface IPlaneView
    {
        event Action AircraftDestroyed;
        event Action CoinTaken;
        
        void DestroyAircraft();
        void SetBody(AircraftBody aircraftBody);

        void Fire();
        void ControlPlane(Vector2 MovementState);
        void MoveForward();
    }
}