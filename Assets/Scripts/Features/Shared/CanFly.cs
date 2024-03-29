using UnityEngine;

namespace Features.Shared
{
    public class CanFly : MonoBehaviour
    {
        private readonly Vector3 _groundPosition = Vector3.zero;
        
        [SerializeField] 
        protected Transform _bodyDirection; 
        [SerializeField]
        public float _speed = 30f;
        
        protected void MoveForwardInternal()
        {
            transform.RotateAround(_groundPosition, _bodyDirection.forward, Time.deltaTime * _speed);
        }
    }
}