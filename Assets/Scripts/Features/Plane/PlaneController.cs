using UnityEngine;

namespace Features.Plane
{
    public class PlaneController : CanFlyController
    {
        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField] 
        private FireController _fireController;
        [SerializeField] 
        private PlaneInputHandler _planeInputHandler;
        private float xParam;
    
        private void Update()
        {
            MoveForward();
            ControlPlane();
            FireIfNeed();
        }

        private void FireIfNeed()
        {
            if (!_planeInputHandler.IsFirePressed)
            {
                return;
            }
            _fireController.Fire();
        }

        // TODO: use consts
        private void ControlPlane()
        {
            var localEulerAngles = _bodyDirection.localEulerAngles;
            xParam = Mathf.Lerp(xParam, _planeInputHandler.MovementState.x, Time.deltaTime * 5f);
            var targetEulerAngles = new Vector3(0f, localEulerAngles.y + xParam * 40f, 0f);
            localEulerAngles = Vector3.Lerp(localEulerAngles, targetEulerAngles, Time.deltaTime * 2f);
            _bodyDirection.localEulerAngles = localEulerAngles;
            Tilt(targetEulerAngles);
        }

        private void Tilt(Vector3 targetEulerAngles)
        {
            var tiltAngle = Mathf.Clamp(Mathf.DeltaAngle(targetEulerAngles.y, _bodyDirection.localEulerAngles.y) * 1.5f, -89f, 89f);
        
            _bodyRotation.localEulerAngles = new Vector3(-tiltAngle, 0, 0);
        }
    }
}