using UnityEngine;

namespace Plane
{
    public class PlaneController : MonoBehaviour
    {
        private readonly Vector3 _groundPosition = Vector3.zero;
        
        [SerializeField] 
        private Transform _bodyDirection;
        [SerializeField] 
        private Transform _bodyRotation;
        [SerializeField]
        public float _speed = 30f;
        [SerializeField] 
        private MovementController _movementController;
        private float xParam;
    
        private void Update()
        {
            MoveForward();
            ControlPlane();
        }

        // TODO: use consts
        private void ControlPlane()
        {
            var localEulerAngles = _bodyDirection.localEulerAngles;
            xParam = Mathf.Lerp(xParam, _movementController.MovementState.x, Time.deltaTime * 5f);
            var targetEulerAngles = new Vector3(0f, localEulerAngles.y + xParam * 40f, 0f);
            localEulerAngles = Vector3.Lerp(localEulerAngles, targetEulerAngles, Time.deltaTime * 2f);
            _bodyDirection.localEulerAngles = localEulerAngles;
            Tilt(targetEulerAngles);
        }

        private void MoveForward() => transform.RotateAround(_groundPosition, _bodyDirection.forward, Time.deltaTime * _speed);

        private void Tilt(Vector3 targetEulerAngles)
        {
            var tiltAngle = Mathf.Clamp(Mathf.DeltaAngle(targetEulerAngles.y, _bodyDirection.localEulerAngles.y) * 1.5f, -89f, 89f);
        
            _bodyRotation.localEulerAngles = new Vector3(-tiltAngle, 0, 0);
        }
    }
}