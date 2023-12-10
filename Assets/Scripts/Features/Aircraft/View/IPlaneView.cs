using Features.Aircraft.Components;

namespace Features.Aircraft.View
{
    public interface IPlaneView
    {
        void DestroyPlane();
        void SetBody(AircraftBody aircraftBody);
    }
}