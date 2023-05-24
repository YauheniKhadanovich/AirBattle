using UnityEngine;

namespace Plane
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] 
        private Transform planeBody;
        [SerializeField] 
        private Transform bodyElements;
        [SerializeField]
        public float speed = 30f;
        [SerializeField] 
        private Transform groundPos;
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
            var localEulerAngles = bodyElements.localEulerAngles;
            xParam = Mathf.Lerp(xParam, _movementController.MovementState.x, Time.deltaTime * 5f);
            var targetEulerAngles = new Vector3(0f, localEulerAngles.y + xParam * 40f, 0f);
            localEulerAngles = Vector3.Lerp(localEulerAngles, targetEulerAngles, Time.deltaTime * 2f);
            bodyElements.localEulerAngles = localEulerAngles;
            Tilt(targetEulerAngles);
        }

        private void MoveForward() => transform.RotateAround(groundPos.position, bodyElements.forward, Time.deltaTime * speed);

        private void Tilt(Vector3 targetEulerAngles)
        {
            var tiltAngle = Mathf.Clamp(Mathf.DeltaAngle(targetEulerAngles.y, bodyElements.localEulerAngles.y) * 1.5f, -89f, 89f);
        
            planeBody.localEulerAngles = new Vector3(-tiltAngle, 0, 0);
        }
    }
}