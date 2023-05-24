using UnityEngine;

namespace Features.Bots.Impl
{
    public class AerostatController : Bot
    {
        [SerializeField] 
        private Transform _bodyRotationAroundSelf;

        private void Update()
        {
            MoveForward();
            RotateAroundSelf();
        }

        private void RotateAroundSelf()
        {
            _bodyRotationAroundSelf.Rotate(Vector3.back * 2f * Time.deltaTime, Space.Self);
        }
        
    }
}